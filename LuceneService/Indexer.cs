
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneService
{
    public class Indexer
    {
        private IndexWriter writer;
        Lucene.Net.Store.Directory dir;
        public Indexer(string indexDir) {
            dir = FSDirectory.Open(indexDir);
            //stop words can be addede to the analyzer 3la fekra :D
            writer = new IndexWriter(dir, new Lucene.Net.Analysis.AR.ArabicAnalyzer(Lucene.Net.Util.Version.LUCENE_30), IndexWriter.MaxFieldLength.UNLIMITED);
        }




        public int Index(IEnumerable<NewsItem> data)
        {
            foreach (var item in data)
	        {
                IndexFile(item);

	        }

            return writer.NumDocs();
        }

        private void IndexFile(NewsItem item)
        {

            Document doc = new Document();
            string content = item.Content;
            string title = item.Title;
            doc.Add(new Field("Content" , content , Field.Store.YES , Field.Index.ANALYZED , Field.TermVector.NO));
            doc.Add(new Field("Title", title, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.NO));
            doc.Add(new Field("ID", item.ID.ToString(), Field.Store.YES, Field.Index.NO , Field.TermVector.NO));

            writer.AddDocument(doc);
        }

        public void Close()
        {

            writer.Optimize();
            writer.Flush(true, false, true);
            writer.Dispose();
            dir.Dispose();
        }
    }
}


