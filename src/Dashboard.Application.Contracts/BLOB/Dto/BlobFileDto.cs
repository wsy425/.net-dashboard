namespace Dashboard.BLOB.Dto
{
    public class BlobFileDto
    {
        public byte[] Bytes { get; set; }
        public bool IsFileNull => Bytes == null || Bytes.Length == 0;
    }
}