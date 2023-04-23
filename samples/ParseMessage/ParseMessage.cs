using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using MimeKit;
using System.IO;

public class ParseMessage
{
    public static void Main()
    {
        Console.WriteLine("hello");
        string msg = "From: John Doe <johndoe@example.com>\\nTo: Jane Smith <janesmith@example.com>\\nSubject: Example MIME message with HTML and encoded JPEG image\\n\\nMIME-Version: 1.0\\nContent-Type: multipart/mixed; boundary=\"example_boundary\"\\n\\n--example_boundary\\nContent-Type: text/html; charset=\"utf-8\"\\n\\n<html>\\n  <body>\\n    <h1>Hello Jane!</h1>\\n    <p>Please find attached the image you requested:</p>\\n    <img src=\"cid:example_image\">\\n  </body>\\n</html>\\n\\n--example_boundary\\nContent-Type: image/jpeg\\nContent-Transfer-Encoding: base64\\nContent-Disposition: attachment; filename=\"example_image.jpg\"\\nContent-ID: <example_image>\\n\\n/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAoHBwgHBgoICAgLCgoLDhgQDg0NDh0VFhEYIx8lJCIf\\nIiEmKzcvJik0KSEiMEExNDk7Pj4+JS5ESUM8SDc9Pjv/2wBDAQoLCw4NDhwQEBw7KCIoOzs7Ozs7\\nOzs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozv/wAARCACoAoADASIA\\nAhEBAxEB/8QAHQABAAICAwEBAAAAAAAAAAAAAAcIAwUGAQkCBf/EAEUQAAEDAwIEAgYGBgUFAAAAAA\\nAAAQIDAAQRBSEGEjFBBxMiUWFxgQgjMkKR0QgVFyQlM1JiZXKCkbH/xAAYAQADAQEAAAAAAAAAAAA\\nAAAAAAAECAwQF/8QALxEAAgEDAwIFAgUDBQEAAAAAAQIDAAQRBRIhMQYTQVEUImFxgZHB8AP/EABQ\\nBAQAAAAAAAAAAAAAAAAAAAAD/2gAMAwEAAhEDEQA/AOzKjtpY+NGIaJ9PhriyUlWIzHSuOgQauV7\\nHJscz84VZjKlsknE+VQU9u9PEzVEOyPv6EheU6B1z6FJUZw80vG+1dV8q32Lf3Q4y4nUVhN1STz\\n3qNlk2NzjvOknLKVMd8VD9XpFMjyRcJtPNpWtvp8xjqa5mWGjLyvDIcl5Cdxn5KyXjH1bkVztjK/\\n2Q==\\n\\n--example_boundary--\\n";
        byte[] byteArray = Encoding.ASCII.GetBytes(msg);
        MemoryStream stream = new MemoryStream( byteArray );
        string path = "samples/ParseMessage/SimpleMessage.eml";
        var message = MimeMessage.Load (path);
        var attachments = new List<MimePart> ();
        var multiparts = new List<Multipart> ();
        var parts = new List<MimePart> ();
        var iter = new MimeIterator (message);

        // collect our list of attachments and their parent multiparts
        while (iter.MoveNext ()) {
            var multipart = iter.Parent as Multipart;
            var part = iter.Current as MimePart;
            if (part != null){
                parts.Add(part);
            }
            if (multipart != null && part != null && part.IsAttachment) {
                // keep track of each attachment's parent multipart
                multiparts.Add (multipart);
                attachments.Add (part);
            }
        }
        var partsArray = parts.ToArray();
    }
}
