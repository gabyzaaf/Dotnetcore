using System;
using Candidate_Management.CORE.Exceptions;
/*
    Author : ZAAFRANI Gabriel
    Version : 1.0
 */
namespace Candidate_Management.CORE.Remind
{   
    /// <summary>
    /// Create the factory with reflexion
    /// </summary>
    public class FactoryRemind
    {
        private static readonly string className = "FactoryRemind";
        public static Iremind createRemind(string remindString){
            try{
                string remindToLoad = $"Candidate_Management.CORE.Remind.{remindString}";
                
                return (Iremind)System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(remindToLoad);
                
            }catch(Exception exc){
                throw new RemindCustomException(className,exc.Message);
            }
        }
    }
}