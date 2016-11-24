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

* `EnvironmentHelper.GetUmbracoEnvironment()` returning `"local", "development", "staging", "live"` (or `"elsewhere"` if unknown).


##### Command-line utility

A command-line utility to set-up a local git repository for an Umbraco Cloud project.

The application will clone the Development environment, then add remote repository references for the Staging and Live environments.


---

### References

This version can be found at https://github.com/leekelleher/umbraco-cloud-toolkit

### License

Copyright &copy; 2014 Lee Kelleher, Umbrella Inc Ltd

This project is licensed under [MIT](http://opensource.org/licenses/MIT), please see [LICENSE](LICENSE.md) for further details.
