namespace Specs
{
    using System;
    using System.Reactive.Disposables;

    public static class EnvironmentVariable
    {
        public static IDisposable UserLevel(string variable, string value)
        {
            return Using(variable, value, EnvironmentVariableTarget.User);
        }

        public static IDisposable ProcessLevel(string variable, string value)
        {
            return Using(variable, value, EnvironmentVariableTarget.Process);
        }

        public static IDisposable Using(string variable, string value, EnvironmentVariableTarget target)
        {
            Environment.SetEnvironmentVariable(
                variable: variable,
                value: value,
                target: target);

            return Disposable.Create(() =>
            {
                Environment.SetEnvironmentVariable(
                variable: variable,
                value: null,
                target: target);
            });
        }
    }
}