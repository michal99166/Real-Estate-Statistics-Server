using RESS.Gumtree.DTO;
using Valit;

namespace RESS.Gumtree.Validators
{
    public class GumTreeTopicDtoValidator : IGumTreeDtoValidator
    {
        public IValitResult Validate(GumtreeTopicDto dto, IValitStrategy strategy = null)
            => ValitRules<GumtreeTopicDto>
                .Create()
                .WithStrategy(s => s.Complete)
                .Ensure(c => c.Id, _ => _
                    .IsNotEmpty())
                .Ensure(c => c.Title, _ => _
                    .Required())
                .Ensure(c => c.Url, _ => _
                    .Required())
                .For(dto)
                .Validate();

    }
}