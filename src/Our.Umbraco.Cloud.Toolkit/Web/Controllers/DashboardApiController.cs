using System;
using System.IO;
using System.Linq;
using System.Web.Http;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using Umbraco.Core.Models;
using Umbraco.Core.Models.EntityBase;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Our.Umbraco.Cloud.Toolkit
{
    [PluginController("UmbracoCloudToolkit")]
    public class DashboardApiController : UmbracoAuthorizedApiController
    {
        [HttpGet]
        public object LookupEntity(string entityId)
        {
            IUmbracoEntity entity = null;
            Guid parsedGuid;
            int parsedId;
            Uri parsedUri;

            if (Guid.TryParse(entityId, out parsedGuid))
            {
                entity = Services.EntityService.GetByKey(parsedGuid);
            }
            else if (int.TryParse(entityId, out parsedId))
            {
                entity = Services.EntityService.Get(parsedId);
            }
            else if ((entityId.InvariantStartsWith("http") || entityId.StartsWith("/")) && Uri.TryCreate(entityId, UriKind.RelativeOrAbsolute, out parsedUri))
            {
                if (!parsedUri.IsAbsoluteUri)
                {
                    parsedUri = parsedUri.MakeAbsolute(Request.RequestUri);
                }

                // Check if media - assumption is only media paths would have a file extension
                var path = parsedUri.GetAbsolutePathDecoded();
                if (Path.HasExtension(path))
                {
                    entity = Services.MediaService.GetMediaByPath(path);
                }
                else
                {
                    // TODO: [LK:2017-07-06] Review if we need to check for the domain, and prefix the route with the domain's ID
                    var content = UmbracoContext.ContentCache.GetByRoute(path);
                    if (content != null)
                    {
                        entity = Services.EntityService.Get(content.Id);
                    }
                }
            }

            if (entity == null)
            {
                return null;
            }

            var result = new LookupResult
            {
                Id = entity.Id,
                Key = entity.Key,
                Name = entity.Name,
                Path = entity.Path.Split(',').Select(int.Parse).ToArray()
            };

            // check if the entity has the "NodeObjectType" data, otherwise get it from the entity service
            var nodeType = entity.AdditionalData.ContainsKey("NodeObjectType") && !string.IsNullOrWhiteSpace(entity.AdditionalData["NodeObjectType"].ToString())
                ? entity.AdditionalData["NodeObjectType"].ToString()
                : Services.EntityService.GetObjectType(entity).GetGuid().ToString();

            if (!string.IsNullOrWhiteSpace(nodeType))
            {
                switch (nodeType.ToUpper())
                {
                    case Constants.ObjectTypes.Document:
                        result.Type = "Content";
                        result.EditUrl = string.Format("#/content/content/edit/{0}", entity.Id);
                        break;

                    case Constants.ObjectTypes.DocumentType:
                        result.Type = "DocumentType";
                        result.EditUrl = UmbracoVersion.Current >= new Version(7, 4, 0)
                            ? string.Format("#/settings/documentTypes/edit/{0}", entity.Id)
                            : string.Format("#/settings/framed/%252Fumbraco%252Fsettings%252FeditNodeTypeNew.aspx%253Fid%253D{0}", entity.Id);
                        break;

                    case Constants.ObjectTypes.Template:
                        result.Type = "Template";
                        result.EditUrl = string.Format("#/settings/framed/%252Fumbraco%252Fsettings%252Fviews%252FeditView.aspx%253FtreeType%253Dtemplates%2526templateID%253D{0}", entity.Id);
                        break;

                    case Constants.ObjectTypes.Media:
                        result.Type = "Media";
                        result.EditUrl = string.Format("#/media/media/edit/{0}", entity.Id);
                        break;

                    case Constants.ObjectTypes.MediaType:
                        result.Type = "MediaType";
                        result.EditUrl = UmbracoVersion.Current >= new Version(7, 4, 0)
                            ? string.Format("#/settings/mediaTypes/edit/{0}", entity.Id)
                            : string.Format("#/settings/framed/%252Fumbraco%252Fsettings%252FeditMediaType.aspx%253Fid%253D{0}", entity.Id);
                        break;

                    case Constants.ObjectTypes.ContentItem:
                        result.Type = "ContentItem";
                        break;

                    case Constants.ObjectTypes.ContentItemType:
                        result.Type = "ContentItemType";
                        break;

                    case Constants.ObjectTypes.Member:
                        result.Type = "Member";
                        result.EditUrl = string.Format("#/member/member/edit/{0}", entity.Id);
                        break;

                    case Constants.ObjectTypes.MemberType:
                        result.Type = "MemberType";
                        result.EditUrl = UmbracoVersion.Current >= new Version(7, 4, 0)
                            ? string.Format("#/member/memberTypes/edit/{0}", entity.Id)
                            : string.Format("#/member/framed/%252Fumbraco%252Fmembers%252FeditMemberType.aspx%253Fid%253D{0}", entity.Id);
                        break;

                    case Constants.ObjectTypes.MemberGroup:
                        result.Type = "MemberGroup";
                        result.EditUrl = string.Format("#/member/framed/%252Fumbraco%252Fmembers%252FeditMemberGroup.aspx%253Fid%253D{0}", entity.Name);
                        break;

                    case Constants.ObjectTypes.Stylesheet:
                        result.Type = "Stylesheet";
                        result.EditUrl = string.Format("#/settings/framed/%252Fumbraco%252Fsettings%252Fstylesheet%252FeditStylesheet.aspx%253Fid%253D{0}", entity.Id);
                        break;

                    case Constants.ObjectTypes.DataType:
                        result.Type = "DataType";
                        result.EditUrl = UmbracoVersion.Current >= new Version(7, 4, 0)
                            ? string.Format("#/developer/dataTypes/edit/{0}", entity.Id)
                            : string.Format("#/developer/datatype/edit/{0}", entity.Id);
                        break;

                    case Constants.ObjectTypes.ContentRecycleBin:
                        result.Type = "ContentRecycleBin";
                        break;

                    case Constants.ObjectTypes.MediaRecycleBin:
                        result.Type = "MediaRecycleBin";
                        break;

                    case Constants.ObjectTypes.SystemRoot:
                        result.Type = "SystemRoot";
                        break;
                }
            }

            return result;
        }
    }
}