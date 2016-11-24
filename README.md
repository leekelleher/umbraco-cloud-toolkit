# Umbraco Cloud Community Toolkit

[![Build status](https://img.shields.io/appveyor/ci/leekelleher/umbraco-cloud-toolkit.svg)](https://ci.appveyor.com/project/leekelleher/umbraco-cloud-toolkit)
[![NuGet release](https://img.shields.io/nuget/v/Our.Umbraco.Cloud.Toolkit.svg)](https://www.nuget.org/packages/Our.Umbraco.Cloud.Toolkit)
[![Our Umbraco project page](https://img.shields.io/badge/our-umbraco-orange.svg)](https://our.umbraco.org/projects/backoffice-extensions/umbraco-cloud-toolkit)

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

## Getting Started

### Installation

> *Note:* Umbraco Cloud Toolkit for Umbraco has been developed against **Umbraco v7.2.8** and will support that version and above.

Umbraco Cloud Toolkit can be installed from either Our Umbraco or NuGet package repositories, or build manually from the source-code:

#### Our Umbraco package repository

To install from Our Umbraco, please download the package from:

> <https://our.umbraco.org/projects/backoffice-extensions/umbraco-cloud-toolkit>

#### NuGet package repository

To [install from NuGet](https://www.nuget.org/packages/Our.Umbraco.Cloud.Toolkit), you can run the following command from within Visual Studio:

	PM> Install-Package Our.Umbraco.Cloud.Toolkit

We also have a [MyGet package repository](https://www.myget.org/gallery/umbraco-packages) - for bleeding-edge / development releases.

#### Manual build

If you prefer, you can compile the project yourself, you'll need:

* Visual Studio 2012 (or above)

To clone it locally click the "Clone in Windows" button above or run the following git commands.

	git clone https://github.com/leekelleher/umbraco-cloud-toolkit.git umbraco-cloud-toolkit
	cd umbraco-cloud-toolkit
	.\build.cmd

---

## Contributing to this project

Anyone and everyone is welcome to contribute. Please take a moment to review the [guidelines for contributing](CONTRIBUTING.md).

* [Bug reports](CONTRIBUTING.md#bugs)
* [Feature requests](CONTRIBUTING.md#features)
* [Pull requests](CONTRIBUTING.md#pull-requests)


## Contact

Have a question?

* [Raise an issue](https://github.com/leekelleher/umbraco-cloud-toolkit/issues) on GitHub


## Dev Team

* [Lee Kelleher](https://github.com/leekelleher)
* [Matt Brailsford](https://github.com/mattbrailsford)


## License

Copyright &copy; 2016 Our Umbraco and [other contributors](https://github.com/leekelleher/umbraco-cloud-toolkit/graphs/contributors)

Copyright &copy; 2014 Lee Kelleher, Umbrella Inc

Licensed under the [MIT License](LICENSE.md)


### Credits

The package logo adapts the following icons from the [Noun Project](https://thenounproject.com), licensed under [CC BY 3.0 US](https://creativecommons.org/licenses/by/3.0/us/):

- [Cloud](https://thenounproject.com/term/cloud/677923/) by [Aya Sofya](https://thenounproject.com/ayasofya/)
- [Wrench](https://thenounproject.com/term/tools/688809/) by [Gregor Črešnar](https://thenounproject.com/grega.cresnar/)
