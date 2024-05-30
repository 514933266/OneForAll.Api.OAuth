### OneForAll登录授权服务
基于IdentityServer4组件封装的登录授权服务库，同时集成手机号登录、微信快捷登录等多种方式。

### 默认登录方式
请求地址：http://localhost:51512/connect/token
请求方式：POST x-www-form-urlencoded
请求参数：
|名称|值|描述|
|-|-|-|
|grant_type|password|oauth2的账号密码登录方式|
|client_Id|OneForAll|客户端id，具体可在表SysClient配置|
|client_secret|OneForAll|客户端密码，具体可在表SysClient配置|
|username|username|登录账号|
|password|password|经过md5加密后的登录密码|

### 微信小程序登录
请求地址：http://localhost:51512/api/WxmpLogin
请求方式：POST json
请求参数：
```json
{
    "Code": "微信调起获取的code值",
    "MobileCode": "微信调起获取的用户手机号码code值",
    "Client": {
        "Id": "客户端id，具体可在表SysClient配置",
        "Secret": "客户端密码，具体可在表SysClient配置",
        "Role": "admin" // 默认全部admin，暂不支持自定义按角色授权
    }
}
```
### 手机号获取验证码
请求地址：http://localhost:51512/api/MobileLogin/Code
请求方式：POST json
请求参数：
```json
"手机号码"
```
**注：验证码功能需要对接部署OneForAll.Api.Ums服务**

### 手机号登录
请求地址：http://localhost:51512/api/MobileLogin
请求方式：POST json
请求参数：
```json
{
    "Code": "手机验证码，需要先对接ums服务获取验证码",
    "PhoneNumber": "手机号码",
    "Client": {
        "Id": "客户端id，具体可在表SysClient配置",
        "Secret": "客户端密码，具体可在表SysClient配置"
    }
}
```
