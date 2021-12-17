using System.ComponentModel.DataAnnotations;

namespace Dashboard.BLOB.Dto
{
    public class BlobUploadInputDto
    {
        [Required]
        public byte[] Bytes { get; set; }
        [Required]
        public string Name { get; set; }
    }
}