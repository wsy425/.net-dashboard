using Dashboard.BLOBConstant;
using Volo.Abp.BlobStoring;

namespace Dashboard.BLOB
{
    // [BlobContainerName("profile_background")]
    [BlobContainerName(nameof(BlobContainerNames.BackGroundContainerName))]
    public abstract class ProfileBackgroundContainer
    {
        
    }

    [BlobContainerName(nameof(BlobContainerNames.InformationContainerName))]
    public abstract class ProfileInfoContainer
    {
        
    }
}