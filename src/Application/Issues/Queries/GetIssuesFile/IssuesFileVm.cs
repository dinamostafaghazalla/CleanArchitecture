namespace CleanArchitecture.Application.Issues.Queries.GetIssuesFile
{
    public class IssuesFileVm
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}
