using IntraPortal.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;

namespace IntraPortal.Models
{
     public enum DisplayMsg 
    {
         Success=1,
         DbError=2,
         InvalidStaffNumber=3

    }
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "スタッフナンバー")]
        public string StaffNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "パスワード")]
        public string Password { get; set; }

        public int Result { get; set; }

        private clsDb db = new clsDb();
        private string strcon = ConfigurationManager.ConnectionStrings["conGps"].ConnectionString;
        public RegisterViewModel GetUserById(int staffNumber)
        {
            string msg = "";
            db.SetbolConnection(strcon, ref msg);
            DataTable user = db.GetdtTableSelectData("Exec spGetDetailsById " + staffNumber, ref msg);
            var regViewModel = new RegisterViewModel();
            foreach (DataRow row in user.Rows)
            {
                regViewModel.CompanyName = row["companyName"].ToString();
                regViewModel.StaffNumber = row["staffNo"].ToString();
                regViewModel.Email = row["email"].ToString();
                regViewModel.Name  = row["name"].ToString();
            }
            return regViewModel;
        }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "電子メール")]
        public string Email { get; set; }

        [Display(Name = "名前")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "スタッフナンバー")]
        public string StaffNumber { get; set; }

        [Display(Name = "会社名")]
        public string CompanyName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} の長さは {2} 文字以上である必要があります。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "パスワード")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "パスワードの確認入力")]
        [Compare("Password", ErrorMessage = "パスワードと確認のパスワードが一致しません。")]
        public string ConfirmPassword { get; set; }

        private clsDb db = new clsDb();
        private string strcon = ConfigurationManager.ConnectionStrings["conGps"].ConnectionString;

        public int IsAdded { get; set; }
      
        public int IsValidStaffNumber(string staffNum)
        {
            int rslt = 0;
            string chkStfNoSql = "Select staffNo from users where staffNo='" + staffNum + "'";
            string msg = "";
            db.SetbolConnection(strcon, ref msg);
            var i = db.GetstrSingleColumnSelectData(chkStfNoSql);
            if(i==staffNum)
            {
                rslt= Convert.ToInt16(DisplayMsg.InvalidStaffNumber);
            }
            else if(i=="")
            {
                rslt =  Convert.ToInt16(DisplayMsg.DbError);
            }
            return rslt;
         
            
        }

        public int RegisterUser(RegisterViewModel user)
        {
            string msg = "";
            string instSql = " Insert into users(email,password,name,staffNo,companyName) values ('" + user.Email + "','" + user.Password + "','" + user.Name + "','" + user.StaffNumber + "','" + user.CompanyName + "')";
            db.SetbolConnection(strcon, ref msg);

            if (db.GetshoExecuteNonQuery(instSql, ref msg) != -1)
            {

               // user.IsAdded = Convert.ToInt16(DisplayMsg.Success);

                return Convert.ToInt16(DisplayMsg.Success);
            }

            else
            {
                //ModelState.Clear();
                //model.IsAdded = Convert.ToInt16(DisplayMsg.DbError);
                //return View(model);
                return Convert.ToInt16(DisplayMsg.DbError);
            }
        }

        public int UpdateUser(RegisterViewModel loggedinUser)
        {
           
            string msg = "";
            string updateUserSql = "exec spUpdateUser '" + loggedinUser.Email + "','" + loggedinUser.Name + "','" + loggedinUser.StaffNumber + "','" + loggedinUser.CompanyName+"'";
            
            db.SetbolConnection(strcon, ref msg);
            


            if (db.GetshoExecuteNonQuery(updateUserSql, ref msg) != -1)
            {
               
                //loggedinUser.IsAdded = 1;

                return Convert.ToInt16(DisplayMsg.Success);
            }

            else
            {
              
                //loggedinUser.IsAdded = 2;
                return Convert.ToInt16(DisplayMsg.DbError);
            }

            
           
        }


        
    }



  
}
