# Umbraco Cloud Community Toolkit

The Umbraco Cloud Toolkit is comprised of the following features...

##### Razor helper extension methods:

* `@Umbraco.IsDevelopment()`
* `@Umbraco.IsStaging()`
* `@Umbraco.IsLive()`

	
##### Developer section: Umbraco Cloud Dashboard

* Node Id -> Guid lookup
* Node Guid -> Id lookup
* Courier - Force Deploy
* Courier - Force XML Rebuild


##### C# Helper methods

* `EnvironmentHelper.GetUmbracoEnvironment()` returning `"local"`, `"development"`, `"staging"`, `"live"` (or `"elsewhere"` if unknown).


##### Command-line utility

A command-line utility to set-up a local git repository for an Umbraco Cloud project.

The application will clone the Development environment, then add remote repository references for the Staging and Live environments.


---

## License

Copyright &copy; 2016 Our Umbraco and [other contributors](https://github.com/leekelleher/umbraco-bulk-user-admin/graphs/contributors)

Copyright &copy; 2014 Lee Kelleher, Umbrella Inc

Licensed under the [MIT License](LICENSE.md)

### Credits

The package logo adapts the following icons from the [Noun Project](https://thenounproject.com), licensed under [CC BY 3.0 US](https://creativecommons.org/licenses/by/3.0/us/):

- [Cloud](https://thenounproject.com/term/cloud/677923/) by [Aya Sofya](https://thenounproject.com/ayasofya/)
- [Wrench](https://thenounproject.com/term/tools/688809/) by [Gregor Črešnar](https://thenounproject.com/grega.cresnar/)

