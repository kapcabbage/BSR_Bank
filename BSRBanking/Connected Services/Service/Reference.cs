﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BSRBanking.Service {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Service.IAccountManager")]
    public interface IAccountManager {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountManager/GetHistory", ReplyAction="http://tempuri.org/IAccountManager/GetHistoryResponse")]
        BSRBankingDataContract.Base.HistoryListResultDto GetHistory(int bankAccountId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountManager/GetHistory", ReplyAction="http://tempuri.org/IAccountManager/GetHistoryResponse")]
        System.Threading.Tasks.Task<BSRBankingDataContract.Base.HistoryListResultDto> GetHistoryAsync(int bankAccountId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountManager/GetBankAccount", ReplyAction="http://tempuri.org/IAccountManager/GetBankAccountResponse")]
        BSRBankingDataContract.Base.BankResultDto GetBankAccount(int userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountManager/GetBankAccount", ReplyAction="http://tempuri.org/IAccountManager/GetBankAccountResponse")]
        System.Threading.Tasks.Task<BSRBankingDataContract.Base.BankResultDto> GetBankAccountAsync(int userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountManager/Transfer", ReplyAction="http://tempuri.org/IAccountManager/TransferResponse")]
        BSRBankingDataContract.Base.BoolResultDto Transfer(BSRBankingDataContract.Dtos.AccountActionDto action);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountManager/Transfer", ReplyAction="http://tempuri.org/IAccountManager/TransferResponse")]
        System.Threading.Tasks.Task<BSRBankingDataContract.Base.BoolResultDto> TransferAsync(BSRBankingDataContract.Dtos.AccountActionDto action);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IAccountManagerChannel : BSRBanking.Service.IAccountManager, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AccountManagerClient : System.ServiceModel.ClientBase<BSRBanking.Service.IAccountManager>, BSRBanking.Service.IAccountManager {
        
        public AccountManagerClient() {
        }
        
        public AccountManagerClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public AccountManagerClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AccountManagerClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AccountManagerClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public BSRBankingDataContract.Base.HistoryListResultDto GetHistory(int bankAccountId) {
            return base.Channel.GetHistory(bankAccountId);
        }
        
        public System.Threading.Tasks.Task<BSRBankingDataContract.Base.HistoryListResultDto> GetHistoryAsync(int bankAccountId) {
            return base.Channel.GetHistoryAsync(bankAccountId);
        }
        
        public BSRBankingDataContract.Base.BankResultDto GetBankAccount(int userId) {
            return base.Channel.GetBankAccount(userId);
        }
        
        public System.Threading.Tasks.Task<BSRBankingDataContract.Base.BankResultDto> GetBankAccountAsync(int userId) {
            return base.Channel.GetBankAccountAsync(userId);
        }
        
        public BSRBankingDataContract.Base.BoolResultDto Transfer(BSRBankingDataContract.Dtos.AccountActionDto action) {
            return base.Channel.Transfer(action);
        }
        
        public System.Threading.Tasks.Task<BSRBankingDataContract.Base.BoolResultDto> TransferAsync(BSRBankingDataContract.Dtos.AccountActionDto action) {
            return base.Channel.TransferAsync(action);
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Service.IAuthorization")]
    public interface IAuthorization {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAuthorization/AuthenticateUser", ReplyAction="http://tempuri.org/IAuthorization/AuthenticateUserResponse")]
        BSRBankingDataContract.Base.UserResultDto AuthenticateUser(string userName, string passwordHash);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAuthorization/AuthenticateUser", ReplyAction="http://tempuri.org/IAuthorization/AuthenticateUserResponse")]
        System.Threading.Tasks.Task<BSRBankingDataContract.Base.UserResultDto> AuthenticateUserAsync(string userName, string passwordHash);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IAuthorizationChannel : BSRBanking.Service.IAuthorization, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AuthorizationClient : System.ServiceModel.ClientBase<BSRBanking.Service.IAuthorization>, BSRBanking.Service.IAuthorization {
        
        public AuthorizationClient() {
        }
        
        public AuthorizationClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public AuthorizationClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AuthorizationClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AuthorizationClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public BSRBankingDataContract.Base.UserResultDto AuthenticateUser(string userName, string passwordHash) {
            return base.Channel.AuthenticateUser(userName, passwordHash);
        }
        
        public System.Threading.Tasks.Task<BSRBankingDataContract.Base.UserResultDto> AuthenticateUserAsync(string userName, string passwordHash) {
            return base.Channel.AuthenticateUserAsync(userName, passwordHash);
        }
    }
}
