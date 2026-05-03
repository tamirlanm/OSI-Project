namespace OSIkz.Web.Services
{
    public class AIAssistantService
    {
        private readonly List<string> _urgentKeywords = new()
        {
            "burst", "water", "gushing", "short circuit", "smells", 
            "burning", "leak", "stuck", "sparking", "gas", "cold", "fire"
        };

        public string CategorizeRequest(string title, string description)
        {
            string fullText = (title + " " + description).ToLower();
            foreach(var word in _urgentKeywords)
            {
                if (fullText.Contains(word))
                {
                    return "Urgent"; // Positive class
                }
            }
            return "Regular"; // Negative class
        }
    }
}