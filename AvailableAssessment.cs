public class AvailableAssessment
{
    public int AssessmentID { get; set; }
    public string AssessmentName { get; set; }
    public string ModuleName { get; set; }
    public string PartnerName { get; set; }
    public decimal PassPercentage { get; set; }
    public bool HasTaken { get; set; }
    public decimal LastScore { get; set; }
    //public DateTime? LastAttemptDate { get; set; }
}