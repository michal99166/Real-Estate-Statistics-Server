using System;

namespace RESS.Modules.GumTree.Application.Exceptions
{
    public class GumTreeTopicAlreadyExistsException : AppException
    {
        public override string ErrorCode => "gumtreeTopic_already_exists";

        public GumTreeTopicAlreadyExistsException(Guid id) : base($"GumTree Topic with id {id} already exists.")
        {
        }

    }
}