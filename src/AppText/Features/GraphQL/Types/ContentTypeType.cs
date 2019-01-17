using AppText.Features.ContentDefinition;
using GraphQL.Types;

namespace AppText.Features.GraphQL.Types
{
    public class ContentTypeType : ObjectGraphType<ContentType>
    {
        public ContentTypeType()
        {
            Field(c => c.Id);
            Field(c => c.Name);
            Field(c => c.Version);
            Field<ListGraphType<FieldType>>("MetaFields");
            Field<ListGraphType<FieldType>>("ContentFields");
        }
    }
}
