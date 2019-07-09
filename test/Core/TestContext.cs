using System;

namespace AngelORM.Tests.Core
{
    public static class TestContext
    {
        public static void Run(Action<Engine> testAction)
        {
            if (testAction == null) throw new ArgumentNullException(nameof(testAction));

            Engine engine = new Engine(Settings.DatabaseConnectionString);

            using (Transaction transaction = engine.BeginTransaction())
            {
                try
                {
                    testAction.Invoke(engine);

                    transaction.Rollback();
                }
                catch
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }
    }
}
