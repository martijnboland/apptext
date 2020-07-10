namespace AppText.Storage.LiteDb
{
    /// <summary>
    /// Options for the LiteDB storage implementation.
    /// </summary>
    public class LiteDbStorageOptions
    {
        /// <summary>
        /// Connection string for the LiteDB database
        /// </summary>
        /// <example>FileName=Physical_File_Path_Of_The_Database;Mode=Exclusive</example>
        public string ConnectionString { get; set; }
    }
}