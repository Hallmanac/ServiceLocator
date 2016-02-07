using System;


namespace Hallmanac.ServiceLocator.Core
{
    public class HallmanacIoc
    {
        public static IServiceLocator Current { get; set; }


        public static void SetServiceLocator(Func<IServiceLocator> create)
        {
            if (create == null)
            {
                throw new ArgumentNullException(nameof(create));
            }
            Current = create();
        }
    }
}