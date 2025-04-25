namespace GraphicalWPF
{
    class TypeObject
    {
        private string type;
        private string filePath;

        public TypeObject(string type, string filePath)
        { 
            this.type = type;
            this.filePath = filePath;
        }

        public string getfilePath() 
        {
            return this.filePath; 
        }
    }
}
