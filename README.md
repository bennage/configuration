# CX

The configuration experience in my .NET projects has been painful.

I want a solution that:

- prevents me from accidentally checking in secrets in source control
- allows individual team members to have their own independent settings
- provides useful feedback about missing settings
- has little ceremony
- is flexible and composable
- plays well with Azure

This repo is about experimenting to find a better solution.

My preferred approach is to have a strongly-typed _configuration object_ that
is populated when the app bootstraps. My experiments are going to center around
that, but I'm open to arguments favoring other approaches.

## proposed syntax
``` C#
// define a configuration
public interface IMyConfiguration
{
    string ConnectionString { get; }
    TimeSpace DurationOfThing { get; }
    int NumberOfSomething { get; set; }
}

// load the config during bootstrapping
var config = Configuration.For<IMyConfiguration>(c =>
{
    // default fallback values
    c.NumberOfSomething = 42;
});
```

Some prefer a different approach to retrieving configuration values. This
uses the same internal mechanism for retrieving the values.
## alternate syntax
``` C#
int numberOfSomething = ConfigurationHelper.Get<int>("MyConfiguration.NumberOfSomething");
```
