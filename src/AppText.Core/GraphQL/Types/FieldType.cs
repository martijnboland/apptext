using AppText.Core.ContentDefinition;
using GraphQL.Types;

namespace AppText.Core.GraphQL.Types
{
    public class FieldType : ObjectGraphType<Field>
    {
        public FieldType()
        {
            Field(c => c.Name);
            Field(c => c.IsRequired);
            Field<StringGraphType>("FieldType", resolve: ctx => ctx.Source.FieldType.GetType().Name);
        }
    }
}
