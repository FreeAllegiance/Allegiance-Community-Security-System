using System;

namespace Allegiance.CommunitySecuritySystem.Common.Utility
{
    public class Domain
    {
        public delegate object DomainDelegate(object data);

        public static object Run(string name, DomainDelegate method, params object[] parameters)
        {
            return Run<object>(name, method, parameters);
        }

        public static T Run<T>(string name, DomainDelegate method, params object[] parameters)
        {
            AppDomain domain = null;
            try
            {
                domain      = AppDomain.CreateDomain(name);
                var key     = string.Concat("RESULT", method.GetHashCode());
                var runner  = new Runner(method, key, parameters);

                domain.DoCallBack(new CrossAppDomainDelegate(runner.Invoke));

                return (T)domain.GetData(key);
            }
#if DEBUG
            catch (Exception error)
            {
                System.Console.WriteLine(error);
                throw;
            }
#endif
            finally
            {
                if(domain != null)
                    AppDomain.Unload(domain);
            }
        }

        [Serializable]
        private class Runner : MarshalByRefObject
        {
            public DomainDelegate Method { get; set; }

            public object[] Arguments { get; set; }

            public string Key { get; set; }

            public Runner(DomainDelegate method, string key, object[] args)
            {
                this.Method     = method;
                this.Key        = key;
                this.Arguments  = args;
            }

            public void Invoke()
            {
                AppDomain.CurrentDomain.SetData(Key, Method.DynamicInvoke(Arguments));
            }
        }
    }
}