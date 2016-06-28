using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Compiler.Generate_Java
{
    public class GenerateMain
    {
        public  string ReturnCode(string code, string firstCode)
        {
            var codeToReturn = @"import java.io.*;
import javax.servlet.*;
import javax.servlet.http.*;

// Extend HttpServlet class
public class pascal extends HttpServlet {
  " + firstCode + @"

  private String message;

  public void init() throws ServletException
  {
      // Do required initialization
  }
 
  public void doGet(HttpServletRequest request,
                    HttpServletResponse response)
            throws ServletException, IOException
  {
    String method =  ""Get"";
    // Set response content type
      response.setContentType(""text/html"");

      PrintWriter out = response.getWriter();
    
    " + code + @"
  }
public void doPost(HttpServletRequest request,
                    HttpServletResponse response)
            throws ServletException, IOException
  {
      // Set response content type
     
    response.setContentType(""text/html"");
       String method =  ""Post"";
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
