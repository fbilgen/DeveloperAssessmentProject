using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Models
{
    public static class Seed
    {
        public static void Initialize(NewsContext context)
        {
            context.Database.EnsureCreated();

            //Look for any article.
            if (context.Article.Any())
            {
                return;   // DB has been seeded
            }

            //Article
            var article = new List<Article>
            {
                  new Article { Title= "Google launches Google Assistant on the iPhone",
                                PublishDate = DateTime.Parse("2017-4-19"),
                                Author = "Romain Dillet",
                                Text =@"Watch out Siri, there’s a new kid in town. Google just announced at its I/O developer conference that its personal 
                                        assistant is coming to iOS. You won’t be able to replace Siri with Google Assistant, but you’ll be able to use the
                                        feature in Google’s dedicated app.“Today, I’m excited to announce that Google Assistant is available for iPhone,” 
                                        VP of Engineering Assistant Scott Huffman said. The app isn’t currently available in the App Store as far as I can see,
                                        but it should be there later today. Edit: You can now download Google Assistant in the App Store.Google Assistant
                                        is considered a more powerful voice assistant when you compare it to the current version of Siri. 
                                        It lets you ask more complicated queries and it has third-party integrations. It also lets you control 
                                        your connected devices, as the company just announced new partnerships with third-party companies."},

                  new Article {Title= "Google makes Kotlin a first-class language for writing Android apps",
                                PublishDate = DateTime.Parse("2017-5-12"),
                                Author = "Frederic Lardinois",
                                Text = @"Google today announced that it is making Kotlin, a statically typed programming language for the Java Virtual Machine, a first-class language for writing Android apps. Kotlin’s primary sponsor is JetBrains, the company behind tools like IntelliJ. It’s 100 percent interoperable with Java, which until now was Google’s primary language for writing Android apps (besides C++).
                                        The company also today said that it will launch a foundation for Kotlin(together with JetBrains).JetBrains open - sourced Kotlin back in 2012 and version 1.0 launched just over a year ago.Google’s own Android Studio, it’s worth noting, is based on the JetBrains IntelliJ Java IDE, and the next version of Android Studio(3.0) will support it out of the box.
                                        Because Kotlin is interoperable with Java, you could already write Android apps in the language before, but now Google will put its weight behind the language.Kotlin includes support for a number of features that Java itself doesn’t currently support.
                                        Google noted in a later keynote that this is only an additional language, not a replacement for its existing Java and C++ support."}
                                };

            article.ForEach(s => context.Article.Add(s));
            context.SaveChanges();

        }
    }
}
