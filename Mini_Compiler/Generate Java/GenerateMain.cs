using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Compiler.Generate_Java
{
    static class GenerateMain
    {
        public static string ReturnCode(string code)
        {
            var codeToReturn = @"import java.io.*;
import javax.servlet.*;
import javax.servlet.http.*;

// Extend HttpServlet class
public class HelloWorld extends HttpServlet {
 
  private String message;

  public void init() throws ServletException
  {
      // Do required initialization
  }

  public void doGet(HttpServletRequest request,
                    HttpServletResponse response)
            throws ServletException, IOException
  {
      // Set response content type
      response.setContentType(""text/html"");

      PrintWriter out = response.getWriter();
    
    " + code + @"
  }
  
  public void destroy()
  {
      // do nothing.
  }
}";
            return codeToReturn;
        }
    }
}
