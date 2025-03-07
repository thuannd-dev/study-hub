using TodoWeb.Application.Services;

namespace TodoWeb.Application.Dtos.GuidModel
{
    public class GuidData
    {
        public IGuidGenerator guidGenerator { get; set; }

        public Guid GetGuid()
        {
            return guidGenerator.Generate();
        }
    }
}
