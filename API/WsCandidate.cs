using System.Collections;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using Core.Adapter.Inteface;
using core.success;
using exception.ws;
using Candidate_Management.CORE.LoadingTemplates;
using System.Collections.Generic;
using core.user;
using Candidate_Management.CORE;

namespace Candidate_Management.API
{
    [EnableCors("SiteCorsPolicy")]
    [Route("api/[controller]")]
    public class CandidateController:Controller
    {
        
        /// <summary>
        /// This method will search the candidate by Action
        /// </summary>
        /// <param name="candidateaction">action type</param>
        /// <param name="token">user token</param>
        /// <returns></returns>
        [HttpGet("actions/{candidateaction}/{token}")]
        public IActionResult UserActionFromCandidate(string candidateaction,string token){
            ArrayList candidatListe = null;
            try{
                if(String.IsNullOrEmpty(token)){
                     throw new Exception("Le token est vide");
                }
               
                if(String.IsNullOrEmpty(candidateaction)){
                    throw new Exception("La variable est vide");
                }
                IsqlMethod isql = Factory.Factory.GetSQLInstance("mysql");
                if(!isql.UserCanRead(token)){
                    throw new Exception("Erreur vous n'avez pas les droits necessaires pour lire");
                }
                candidatListe = isql.searchCandidateByAction(candidateaction,token);
                return new ObjectResult(candidatListe);
            }catch(Exception exc){
                new WsCustomeException(this.GetType().Name,exc.Message);
                ArrayList errorList = new ArrayList();
                errorList.Add(new State(){code=4,content=exc.Message,success=false});
                return CreatedAtRoute("GetErrorsCandidate", new { error = errorList },errorList);
            }
        }
        /// <summary>
        /// Send the template title.
        /// I am using this this method the optimisation
        /// </summary>
        /// <param name="token">User token</param>
        /// <param name="lim1">limite 1</param>
        /// <param name="lim2">limite 2</param>
        /// <returns></returns>
        [HttpGet("email/template/titles/{token}/{lim1}/{lim2}")]
        public IActionResult titleCandidate(string token,int lim1,int lim2){
            ArrayList emailTitles = null;
            try{
                if(String.IsNullOrEmpty(token)){
                     throw new Exception("Le token est vide");
                }
                IsqlMethod isql = Factory.Factory.GetSQLInstance("mysql");
                if(!isql.UserCanRead(token)){
                    throw new Exception("Erreur vous n'avez pas les droits necessaires pour lire");
                }
                emailTitles = isql.emailTemplateTiltes(lim1,lim2);
                return new ObjectResult(emailTitles);
            }catch(Exception exc){
                new WsCustomeException(this.GetType().Name,exc.Message);
                ArrayList errorList = new ArrayList();
                errorList.Add(new State(){code=5,content=exc.Message,success=false});
                return CreatedAtRoute("GetErrorsCandidate", new { error = errorList },errorList);
            }
        }
        /// <summary>
        /// You can get the Email template content from the title
        /// </summary>
        /// <param name="token">userToken</param>
        /// <param name="title">Template Title</param>
        /// <returns></returns>
        [HttpGet("template/email/{token}/{title}")]
        public IActionResult contentTemplateEmail(string token,string title){
            
             ArrayList emailContent = null;
             try{
                if(String.IsNullOrEmpty(token)){
                     throw new Exception("Le token est vide");
                }
                IsqlMethod isql = Factory.Factory.GetSQLInstance("mysql");
                if(!isql.UserCanRead(token)){
                        throw new Exception("Erreur vous n'avez pas les droits necessaires pour lire");
                }
                emailContent = isql.emailTemplateContentFromTitle(title);
                return new ObjectResult(emailContent);
             }catch(Exception exc){
                new WsCustomeException(this.GetType().Name,exc.Message);
                ArrayList errorList = new ArrayList();
                errorList.Add(new State(){code=5,content=exc.Message,success=false});
                return CreatedAtRoute("GetErrorsCandidate", new { error = errorList },errorList);
             }
        }
        /// <summary>
        /// Update the email content.
        /// </summary>
        /// <param name="emailTemplate">Template class</param>
        /// <returns></returns>
        [HttpPost("template/email/update")]
        public IActionResult updateContentEmailFromTitle([FromBody]Template emailTemplate){
            
            try{
                
                if(emailTemplate == null){
                    throw new Exception("L'email template n'existe pas");
                }
                
                if(String.IsNullOrEmpty(emailTemplate.token)){
                    throw new Exception("Le token n'existe pas, vous devez le renseigner");
                }
                
                if(String.IsNullOrEmpty(emailTemplate.content)){
                    throw new Exception("Le contenu de l'email n'existe pas ");
                }
                
                if(String.IsNullOrEmpty(emailTemplate.title)){
                    throw new Exception("Le titre du fichier template email n'existe pas");
                }
                
                IsqlMethod isql = Factory.Factory.GetSQLInstance("mysql");
                isql.UserCanUpdate(emailTemplate.token);
                
                bool templateExist = isql.emailTemplateExist(emailTemplate.title);

                if(!templateExist){
                    throw new Exception($"Aucun template existe avec votre titre : {emailTemplate.title}");
                }
                isql.updateTemplateEmailFromTitle(emailTemplate.title,emailTemplate.content);
                return new ObjectResult(new State(){code=4,content="Le template d'email a bien ete modifie",success=true}); 
            }catch(Exception exc){
                new WsCustomeException(this.GetType().Name,exc.Message);
                ArrayList errorList = new ArrayList();
                errorList.Add(new State(){code=5,content=exc.Message,success=false});
                return CreatedAtRoute("GetErrorsCandidate", new { error = errorList },errorList);
            } 
        }
        /// <summary>
        /// Delete the content Email
        /// </summary>
        /// <param name="emailTemplate">Template type</param>
        /// <returns></returns>
        [HttpPost("template/email/delete")]
        public IActionResult deleteContentEmailFromTitle([FromBody]Template emailTemplate){
           
            try{
                if(emailTemplate == null){
                    throw new Exception("L'email template n'existe pas");
                }
                if(String.IsNullOrEmpty(emailTemplate.token)){
                    throw new Exception("Le token n'existe pas, vous devez le renseigner");
                }
                if(String.IsNullOrEmpty(emailTemplate.title)){
                    throw new Exception("Le titre du fichier template email n'existe pas ");
                }
                IsqlMethod isql = Factory.Factory.GetSQLInstance("mysql");
                isql.UserCanDelete(emailTemplate.token);
                bool templateExist = isql.emailTemplateExist(emailTemplate.title);
                if(!templateExist){
                    throw new Exception($"Aucun template existe avec votre titre : {emailTemplate.title}");
                }
                isql.deleteTemplateEmailFromTitle(emailTemplate.title);
                return new ObjectResult(new State(){code=4,content=$"Le template d'email ayant le titre {emailTemplate.title} à bien ete supprime",success=true}); 
            }catch(Exception exc){
                new WsCustomeException(this.GetType().Name,exc.Message);
                ArrayList errorList = new ArrayList();
                errorList.Add(new State(){code=5,content=exc.Message,success=false});
                return CreatedAtRoute("GetErrorsCandidate", new { error = errorList },errorList);
            } 
        }

        
        /// <summary>
        /// Delete Candidat by Email
        /// </summary>
        /// <param name="candidateDelete">CandidateDelete class </param>
        /// <returns></returns>
        [HttpPost("delete/byEmail")]
        public IActionResult deleteCandidateById([FromBody]CandidateDelete candidateDelete){
            try{
                if(candidateDelete == null){
                    throw new Exception("Les informations de l'utilisateur passé en paramètre sont vides");
                }
                if(String.IsNullOrEmpty(candidateDelete.token)){
                    throw new Exception("Le token de l'utilisateur ne peut etre vide");
                }
                if(String.IsNullOrEmpty(candidateDelete.email)){
                    throw new Exception("L'email du candidat ne peut etre vide");
                }
                IsqlMethod isql = Factory.Factory.GetSQLInstance("mysql");
                isql.UserCanDelete(candidateDelete.token);
                ArrayList CandidatesInformations = isql.searchCandidateWithSpecificEmail(candidateDelete.email);
                Dictionary<string,string> CandidateInformation = (Dictionary<string,string>)CandidatesInformations[0];
                int candidateId = Int32.Parse(CandidateInformation["id"]);
                isql.deleteCandidateById(candidateId);
                return new ObjectResult(new State(){code=4,content=$"Le Candidat a bien été supprimé",success=true});
            return new ObjectResult(candidateDelete);
            }catch(Exception exc){
                new WsCustomeException(this.GetType().Name,exc.Message);
                ArrayList errorList = new ArrayList();
                errorList.Add(new State(){code=5,content=exc.Message,success=false});
                return CreatedAtRoute("GetErrorsCandidate", new { error = errorList },errorList);
            } 
        }
        /// <summary>
        /// List all the candidate with limit for optimisation
        /// </summary>
        /// <param name="limite1">limit begin</param>
        /// <param name="limite2">limit end</param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("list/{limite1}/{limite2}/{token}")]
        public IActionResult deleteCandidateById(int limite1,int limite2,string token){
            try{
                if(limite1 < 0){
                    throw new Exception("La limite1 ne peut etre inférrieur à 0");
                }
                if(limite2 <= 0){
                    throw new Exception("La limite2 ne peut etre inférrieur ou égale à 0");
                }
                
                if(String.IsNullOrEmpty(token)){
                    throw new Exception("Le token ne peut pas etre vide");
                }
                IsqlMethod isql = Factory.Factory.GetSQLInstance("mysql");
                isql.UserCanRead(token);
                ArrayList candidateList = isql.getCandidatesListWithLimite(limite1,limite2);
                return new ObjectResult(candidateList);
            }catch(Exception exc){
                new WsCustomeException(this.GetType().Name,exc.Message);
                ArrayList errorList = new ArrayList();
                errorList.Add(new State(){code=5,content=exc.Message,success=false});
                return CreatedAtRoute("GetErrorsCandidate", new { error = errorList },errorList);
            } 
        } 


        /// <summary>
        /// Handle the error exit for the final User.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
       [HttpGet("{error}", Name = "GetErrorsCandidate")]
        public IActionResult ErrorList(ArrayList errors)
        {
            return new ObjectResult(errors);
        }

        
          
      

    }
}