namespace Specs
{
    using System;

    public static class EnvVarHelper
    {
        public static void SetEnvVar(string value, EnvironmentVariableTarget target, string variable)
        {
            Environment.SetEnvironmentVariable(
                variable: variable,
                value: value,
                target: target);
        }
    }
}