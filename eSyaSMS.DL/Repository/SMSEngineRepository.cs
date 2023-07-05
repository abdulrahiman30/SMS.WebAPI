using HCP.SMS.DL;
using HCP.SMS.DL.Entities;
using HCP.SMS.DO;
using HCP.SMS.IF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCP.SMS.DL.Repository
{
    public class SMSEngineRepository : ISMSEngineRepository
    {
        #region SMS Variable

        public async Task<List<DO_SMSVariable>> GetSMSVariableInformation()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcsmsv
                         .Select(r => new DO_SMSVariable
                         {
                             Smsvariable = r.Smsvariable,
                             Smscomponent = r.Smscomponent,
                             ActiveStatus = r.ActiveStatus
                         }).OrderBy(o => o.Smsvariable).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_SMSVariable>> GetActiveSMSVariableInformation()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcsmsv
                        .Where(w => w.ActiveStatus)
                         .Select(r => new DO_SMSVariable
                         {
                             Smsvariable = r.Smsvariable,
                             Smscomponent = r.Smscomponent
                         }).OrderBy(o => o.Smsvariable).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoSMSVariable(DO_SMSVariable obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        bool is_SMSVariableExist = db.GtEcsmsv.Any(a => a.Smsvariable.Trim().ToUpper() == obj.Smsvariable.Trim().ToUpper());
                        if (is_SMSVariableExist)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "SMS Variable is already exist." };
                        }

                        bool is_SMSComponentExist = db.GtEcsmsv.Any(a => a.Smscomponent.Trim().ToUpper() == obj.Smscomponent.Trim().ToUpper());
                        if (is_SMSComponentExist)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "SMS Component is already exist." };
                        }

                        var sm_sv = new GtEcsmsv
                        {
                            Smsvariable = obj.Smsvariable,
                            Smscomponent = obj.Smscomponent,
                            ActiveStatus = obj.ActiveStatus,
                            CreatedBy = obj.UserID,
                            CreatedOn = DateTime.Now,
                            CreatedTerminal = obj.TerminalID

                        };
                        db.GtEcsmsv.Add(sm_sv);

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "SMS Variable Created Successfully." };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> UpdateSMSVariable(DO_SMSVariable obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_SMSComponentExist = db.GtEcsmsv.Where(w => w.Smscomponent.Trim().ToUpper().Replace(" ", "") == obj.Smscomponent.Trim().ToUpper().Replace(" ", "")
                                && w.Smsvariable != obj.Smsvariable).FirstOrDefault();
                        if (is_SMSComponentExist != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "SMS Component is already exist." };
                        }

                        GtEcsmsv sm_sv = db.GtEcsmsv.Where(w => w.Smsvariable == obj.Smsvariable).FirstOrDefault();
                        if (sm_sv == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "SMS Variable is not exist" };
                        }

                        sm_sv.Smscomponent = obj.Smscomponent;
                        sm_sv.ActiveStatus = obj.ActiveStatus;
                        sm_sv.ModifiedBy = obj.UserID;
                        sm_sv.ModifiedOn = DateTime.Now;
                        sm_sv.ModifiedTerminal = obj.TerminalID;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "SMS Variable Updated Successfully." };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> ActiveOrDeActiveSMSVariable(bool status, string smsvariable)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcsmsv sms_var = db.GtEcsmsv.Where(w => w.Smsvariable.Trim().ToUpper().Replace(" ", "") == smsvariable.Trim().ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (sms_var == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "SMS Variable is not exist" };
                        }

                        sms_var.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, Message = "SMS Variable Activated Successfully." };
                        else
                            return new DO_ReturnParameter() { Status = true, Message = "SMS Variable De Activated Successfully." };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));

                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        #endregion SMS Variable

        #region SMS Information
        public async Task<List<DO_Forms>> GetExistingFormsFromSMSHeader()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var result = db.GtEcsmsh.Where(x => x.ActiveStatus == true).Join(db.GtEcfmfd,
                        d => d.FormId,
                        f => f.FormId,
                        (d, f) => new DO_Forms
                        {
                            FormID = d.FormId,
                            FormName = f.FormName,

                        }).GroupBy(x => x.FormID).Select(y => y.First()).Distinct().ToListAsync();
                    return await result;
                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_SMSTEvent>> GetSMSTriggerEvent()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcsmst
                        .Where(w => w.ActiveStatus)
                         .Select(r => new DO_SMSTEvent
                         {
                             TEventID = r.TeventId,
                             TEventDesc = r.TeventDesc,
                             ActiveStatus = r.ActiveStatus
                         }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_SMSHeader>> GetSMSHeaderInformationByFormId(int formId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcsmsh
                        .Where(w => w.FormId == formId)
                         .Select(r => new DO_SMSHeader
                         {
                             Smsid = r.Smsid,
                             Smsdescription = r.Smsdescription,
                             IsVariable = r.IsVariable,
                             TEventID = r.TeventId,
                             Smsstatement = r.Smsstatement,
                             ActiveStatus = r.ActiveStatus
                         }).OrderBy(o => o.Smsid).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_SMSHeader> GetSMSHeaderInformationBySMSId(string smsId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcsmsh
                        .Where(w => w.Smsid == smsId)
                         .Select(r => new DO_SMSHeader
                         {
                             Smsid = r.Smsid,
                             Smsdescription = r.Smsdescription,
                             IsVariable = r.IsVariable,
                             TEventID = r.TeventId,
                             Smsstatement = r.Smsstatement,
                             ActiveStatus = r.ActiveStatus,
                             l_SMSParameter = r.GtEcsmsd.Select(p => new DO_eSyaParameter
                             {
                                 ParameterID = p.ParameterId,
                                 ParmAction = p.ParmAction
                             }).ToList()
                         }).FirstOrDefaultAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoSMSHeader(DO_SMSHeader obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        bool is_SMSDescExist = db.GtEcsmsh.Any(a => a.Smsdescription.Trim().ToUpper() == obj.Smsdescription.Trim().ToUpper());
                        if (is_SMSDescExist)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "SMS Description is already exist." };
                        }

                        var smsIdNumber = db.GtEcsmsh.Where(w => w.FormId == obj.FormId).Count();
                        string smsId = obj.FormId.ToString() + "_" + (smsIdNumber + 1).ToString();

                        var sm_sh = new GtEcsmsh
                        {
                            Smsid = smsId,
                            FormId = obj.FormId,
                            Smsdescription = obj.Smsdescription,
                            IsVariable = obj.IsVariable,
                            TeventId = obj.TEventID,
                            Smsstatement = obj.Smsstatement,
                            ActiveStatus = obj.ActiveStatus,
                            CreatedBy = obj.UserID,
                            CreatedOn = DateTime.Now,
                            CreatedTerminal = obj.TerminalID

                        };
                        db.GtEcsmsh.Add(sm_sh);

                        foreach (var p in obj.l_SMSParameter)
                        {
                            var sm_sd = new GtEcsmsd
                            {
                                Smsid = smsId,
                                ParameterId = p.ParameterID,
                                ParmAction = p.ParmAction,
                                ActiveStatus = obj.ActiveStatus,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalID,
                                CreatedBy = obj.UserID,
                            };
                            db.GtEcsmsd.Add(sm_sd);
                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "SMS Information Created Successfully." };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> UpdateSMSHeader(DO_SMSHeader obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_SMSComponentExist = db.GtEcsmsh.Where(w => w.Smsdescription.Trim().ToUpper().Replace(" ", "") == obj.Smsdescription.Trim().ToUpper().Replace(" ", "")
                                && w.Smsid != obj.Smsid && w.FormId == obj.FormId).FirstOrDefault();
                        if (is_SMSComponentExist != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "SMS Description is already exist." };
                        }

                        GtEcsmsh sm_sh = db.GtEcsmsh.Where(w => w.Smsid == obj.Smsid).FirstOrDefault();
                        if (sm_sh == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "SMS Id is not exist" };
                        }

                        sm_sh.Smsdescription = obj.Smsdescription;
                        sm_sh.IsVariable = obj.IsVariable;
                        sm_sh.TeventId = obj.TEventID;
                        sm_sh.Smsstatement = obj.Smsstatement;
                        sm_sh.ActiveStatus = obj.ActiveStatus;
                        sm_sh.ModifiedBy = obj.UserID;
                        sm_sh.ModifiedOn = DateTime.Now;
                        sm_sh.ModifiedTerminal = obj.TerminalID;

                        foreach (var p in obj.l_SMSParameter)
                        {
                            var sm_sd = db.GtEcsmsd.Where(w => w.Smsid == obj.Smsid && w.ParameterId == p.ParameterID).FirstOrDefault();
                            if (sm_sd == null)
                            {
                                sm_sd = new GtEcsmsd
                                {
                                    Smsid = obj.Smsid,
                                    ParameterId = p.ParameterID,
                                    ParmAction = p.ParmAction,
                                    ActiveStatus = obj.ActiveStatus,
                                    CreatedOn = DateTime.Now,
                                    CreatedTerminal = obj.TerminalID,
                                    CreatedBy = obj.UserID,
                                };
                                db.GtEcsmsd.Add(sm_sd);
                            }
                            else
                            {
                                sm_sd.ParmAction = p.ParmAction;
                                sm_sd.ActiveStatus = obj.ActiveStatus;
                                sm_sd.ModifiedBy = obj.UserID;
                                sm_sd.ModifiedOn = System.DateTime.Now;
                                sm_sd.ModifiedTerminal = obj.TerminalID;
                            }
                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "SMS Information Updated Successfully." };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        #endregion SMS Information

        #region SMS Recipient

        public async Task<List<DO_SMSHeader>> GetSMSHeaderForRecipientByFormIdandParamId(int formId,int parameterId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcsmsd
                        .Where(w => w.ParameterId == parameterId && w.Sms.FormId == formId && w.ActiveStatus && w.Sms.ActiveStatus)
                         .Select(r => new DO_SMSHeader
                         {
                             Smsid = r.Smsid,
                             Smsdescription = r.Sms.Smsdescription,
                             Smsstatement = r.Sms.Smsstatement,
                             ActiveStatus = r.ActiveStatus
                         }).OrderBy(o => o.Smsid).ToListAsync();

                    return await ds;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_SMSRecipient>> GetSMSRecipientByBusinessKeyAndSMSId(int businessKey,string smsId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcsmsr
                        .Where(w => w.BusinessKey == businessKey && w.Smsid == smsId)
                         .Select(r => new DO_SMSRecipient
                         {
                             Smsid = r.Smsid,
                             MobileNumber = r.MobileNumber,
                             RecipientName = r.RecipientName,
                             Remarks = r.Remarks,
                             ActiveStatus = r.ActiveStatus
                         }).OrderBy(o => o.RecipientName).ToListAsync();

                    return await ds;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoSMSRecipient(DO_SMSRecipient obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool is_MobilenumberExist = db.GtEcsmsr.Any(a => a.MobileNumber.Trim() == obj.MobileNumber.Trim() && a.Smsid == obj.Smsid);
                        if (is_MobilenumberExist)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Mobile number is already exist." };
                        }

                        var sm_sr = new GtEcsmsr
                        {
                            BusinessKey = obj.BusinessKey,
                            Smsid = obj.Smsid,
                            MobileNumber = obj.MobileNumber,
                            RecipientName = obj.RecipientName,
                            Remarks = obj.Remarks,
                            ActiveStatus = obj.ActiveStatus,
                            CreatedBy = obj.UserID,
                            CreatedOn = DateTime.Now,
                            CreatedTerminal = obj.TerminalID

                        };
                        db.GtEcsmsr.Add(sm_sr);

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "SMS Recipient Created Successfully." };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> UpdateSMSRecipient(DO_SMSRecipient obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcsmsr sm_sr = db.GtEcsmsr.Where(w => w.BusinessKey == obj.BusinessKey && w.Smsid == obj.Smsid && w.MobileNumber == obj.MobileNumber).FirstOrDefault();
                        if (sm_sr == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Mobile number is not exist" };
                        }

                        sm_sr.RecipientName = obj.RecipientName;
                        sm_sr.Remarks = obj.Remarks;
                        sm_sr.ActiveStatus = obj.ActiveStatus;
                        sm_sr.ModifiedBy = obj.UserID;
                        sm_sr.ModifiedOn = DateTime.Now;
                        sm_sr.ModifiedTerminal = obj.TerminalID;

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "SMS Recipient Updated Successfully." };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }


        #endregion SMS Recipient

        #region Trigger Event

        public async Task<List<DO_SMSTEvent>> GetAllSMSTriggerEvents()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcsmst
                         .Select(t => new DO_SMSTEvent
                         {
                             TEventID = t.TeventId,
                             TEventDesc = t.TeventDesc,
                             ActiveStatus = t.ActiveStatus
                         }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoSMSTriggerEvent(DO_SMSTEvent obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        bool is_SMSTeventExist = db.GtEcsmst.Any(a => a.TeventId == obj.TEventID);
                        if (is_SMSTeventExist)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "SMS Trigger Event is already exist." };
                        }

                        bool is_SMSTdescExist = db.GtEcsmst.Any(a => a.TeventDesc.Trim().ToUpper() == obj.TEventDesc.Trim().ToUpper());
                        if (is_SMSTdescExist)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "SMS Trigger Event Description is already exist." };
                        }

                        var sm_tevnt = new GtEcsmst
                        {
                            TeventId = obj.TEventID,
                            TeventDesc = obj.TEventDesc,
                            ActiveStatus = obj.ActiveStatus,
                            CreatedBy = obj.UserID,
                            CreatedOn = DateTime.Now,
                            CreatedTerminal = obj.TerminalID

                        };
                        db.GtEcsmst.Add(sm_tevnt);

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "SMS Trigger Event Created Successfully." };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> UpdateSMSTriggerEvent(DO_SMSTEvent obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_SMSTEventExist = db.GtEcsmst.Where(w => w.TeventDesc.Trim().ToUpper().Replace(" ", "") == obj.TEventDesc.Trim().ToUpper().Replace(" ", "")
                                && w.TeventId != obj.TEventID).FirstOrDefault();
                        if (is_SMSTEventExist != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "SMS Trigger Event Description is already exist." };
                        }

                        GtEcsmst sm_tevent = db.GtEcsmst.Where(w => w.TeventId == obj.TEventID).FirstOrDefault();
                        if (sm_tevent == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "SMS Trigger Event is not exist" };
                        }

                        sm_tevent.TeventDesc = obj.TEventDesc;
                        sm_tevent.ActiveStatus = obj.ActiveStatus;
                        sm_tevent.ModifiedBy = obj.UserID;
                        sm_tevent.ModifiedOn = DateTime.Now;
                        sm_tevent.ModifiedTerminal = obj.TerminalID;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "SMS Trigger Event Updated Successfully." };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> DeleteSMSTriggerEvent(int TeventId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcsmst sm_tevent = db.GtEcsmst.Where(w => w.TeventId == TeventId).FirstOrDefault();

                        if (sm_tevent == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Couldn't find SMS Trigger Event." };
                        }

                        db.GtEcsmst.Remove(sm_tevent);

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "SMS Trigger Event Deleted Successfully." };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> ActiveOrDeActiveSMSTriggerEvent(bool status, int TriggerEventId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcsmst t_evevt = db.GtEcsmst.Where(w => w.TeventId == TriggerEventId).FirstOrDefault();
                        if (t_evevt == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "SMS Trigger Event is not exist" };
                        }

                        t_evevt.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, Message = "SMS Trigger Event Activated Successfully." };
                        else
                            return new DO_ReturnParameter() { Status = true, Message = "SMS Trigger Event De Activated Successfully." };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));

                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        #endregion SMS Trigger Event
    }
}
