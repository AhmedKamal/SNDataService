using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LuceneService
{
    public class Searcher
    {

        public static List<long> Search(string indexDir, string myquery , int limit)
        {

            Directory dir = FSDirectory.Open(indexDir);
            IndexReader reader = IndexReader.Open(dir, true);

            //int doccount = reader.DocFreq(new Term("Content", "المصريين"));
            //var terms = reader.GetTermFreqVector(4, "Content").GetTerms();
            int count = reader.NumDocs();
            IndexSearcher indexSearcher = new IndexSearcher(dir);
            QueryParser parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Content", new Lucene.Net.Analysis.AR.ArabicAnalyzer(Lucene.Net.Util.Version.LUCENE_30));
            Query query = parser.Parse(myquery);
            TopDocs docs = indexSearcher.Search(query, limit);

            //List<Tuple<int, float>> results = new List<Tuple<int, float>>();

            //foreach (var doc in docs.ScoreDocs)
            //{
            //    results.Add(Tuple.Create<int,float>(doc.Doc , doc.Score));
            //}

            List<long> results = new List<long>();
            foreach (var doc in docs.ScoreDocs)
            {
                Document resDoc = indexSearcher.Doc(doc.Doc);
                Field resDocIDField = resDoc.GetField("ID");
                int resDocID = int.Parse((resDocIDField.StringValue));
                results.Add(resDocID);
            }
            indexSearcher.Dispose();
            return results;
        }
    }
}
