
using System.Reflection;
using QTWithMe.Data;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;

namespace QTWithMe.Extensions
{
    public class UseAppDbContextAttribute : ObjectFieldDescriptorAttribute
    {
        public override void OnConfigure(
            IDescriptorContext context,
            IObjectFieldDescriptor descriptor,
            MemberInfo member)
        {
            descriptor.UseDbContext<AppDbContext>();
        }
    }
}
