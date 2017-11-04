using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DotNetBlog.Common
{
    /// <summary>
    /// Copy of <see cref="System.Security.Claims.ClaimTypes"/>
    /// </summary>
    public static class ClaimTypesConstants
    {
        //
        // 摘要:
        //     http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor.
        public const string Actor = "actor";
        //
        // 摘要:
        //     The URI for a claim that specifies the postal code of an entity, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/postalcode.
        public const string PostalCode = "postalcode";
        //
        // 摘要:
        //     The URI for a claim that specifies the primary group SID of an entity, http://schemas.microsoft.com/ws/2008/06/identity/claims/primarygroupsid.
        public const string PrimaryGroupSid = "primarygroupsid";
        //
        // 摘要:
        //     The URI for a claim that specifies the primary SID of an entity, http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid.
        public const string PrimarySid = "primarysid";
        //
        // 摘要:
        //     The URI for a claim that specifies the role of an entity, http://schemas.microsoft.com/ws/2008/06/identity/claims/role.
        public const string Role = "role";
        //
        // 摘要:
        //     The URI for a claim that specifies an RSA key, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/rsa.
        public const string Rsa = "rsa";
        //
        // 摘要:
        //     The URI for a claim that specifies a serial number, http://schemas.microsoft.com/ws/2008/06/identity/claims/serialnumber.
        public const string SerialNumber = "serialnumber";
        //
        // 摘要:
        //     The URI for a claim that specifies a security identifier (SID), http://schemas.xmlsoap.org/ws/2005/05/identity/claims/sid.
        public const string Sid = "sid";
        //
        // 摘要:
        //     The URI for a claim that specifies a service principal name (SPN) claim, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/spn.
        public const string Spn = "spn";
        //
        // 摘要:
        //     The URI for a claim that specifies the state or province in which an entity resides,
        //     http://schemas.xmlsoap.org/ws/2005/05/identity/claims/stateorprovince.
        public const string StateOrProvince = "stateorprovince";
        //
        // 摘要:
        //     The URI for a claim that specifies the street address of an entity, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/streetaddress.
        public const string StreetAddress = "streetaddress";
        //
        // 摘要:
        //     The URI for a claim that specifies the surname of an entity, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname.
        public const string Surname = "surname";
        //
        // 摘要:
        //     The URI for a claim that identifies the system entity, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/system.
        public const string System = "system";
        //
        // 摘要:
        //     The URI for a claim that specifies a thumbprint, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/thumbprint.
        //     A thumbprint is a globally unique SHA-1 hash of an X.509 certificate.
        public const string Thumbprint = "thumbprint";
        //
        // 摘要:
        //     The URI for a claim that specifies a user principal name (UPN), http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn.
        public const string Upn = "upn";
        //
        // 摘要:
        //     The URI for a claim that specifies a URI, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/uri.
        public const string Uri = "uri";
        //
        // 摘要:
        //     http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata.
        public const string UserData = "userdata";
        //
        // 摘要:
        //     http://schemas.microsoft.com/ws/2008/06/identity/claims/version.
        public const string Version = "version";
        //
        // 摘要:
        //     The URI for a claim that specifies the webpage of an entity, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/webpage.
        public const string Webpage = "webpage";
        //
        // 摘要:
        //     The URI for a claim that specifies the Windows domain account name of an entity,
        //     http://schemas.microsoft.com/ws/2008/06/identity/claims/windowsaccountname.
        public const string WindowsAccountName = "windowsaccountname";
        //
        // 摘要:
        //     http://schemas.microsoft.com/ws/2008/06/identity/claims/windowsdeviceclaim.
        public const string WindowsDeviceClaim = "windowsdeviceclaim";
        //
        // 摘要:
        //     http://schemas.microsoft.com/ws/2008/06/identity/claims/windowsdevicegroup.
        public const string WindowsDeviceGroup = "windowsdevicegroup";
        //
        // 摘要:
        //     http://schemas.microsoft.com/ws/2008/06/identity/claims/windowsfqbnversion.
        public const string WindowsFqbnVersion = "windowsfqbnversion";
        //
        // 摘要:
        //     http://schemas.microsoft.com/ws/2008/06/identity/claims/windowssubauthority.
        public const string WindowsSubAuthority = "windowssubauthority";
        //
        // 摘要:
        //     The URI for a claim that specifies the alternative phone number of an entity,
        //     http://schemas.xmlsoap.org/ws/2005/05/identity/claims/otherphone.
        public const string OtherPhone = "otherphone";
        //
        // 摘要:
        //     The URI for a claim that specifies the name of an entity, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier.
        public const string NameIdentifier = "nameidentifier";
        //
        // 摘要:
        //     The URI for a claim that specifies the name of an entity, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name.
        public const string Name = "name";
        //
        // 摘要:
        //     The URI for a claim that specifies the mobile phone number of an entity, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone.
        public const string MobilePhone = "mobilephone";
        //
        // 摘要:
        //     The URI for a claim that specifies the anonymous user; http://schemas.xmlsoap.org/ws/2005/05/identity/claims/anonymous.
        public const string Anonymous = "anonymous";
        //
        // 摘要:
        //     The URI for a claim that specifies details about whether an identity is authenticated,
        //     http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authenticated.
        public const string Authentication = "authentication";
        //
        // 摘要:
        //     The URI for a claim that specifies the instant at which an entity was authenticated;
        //     http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant.
        public const string AuthenticationInstant = "authenticationinstant";
        //
        // 摘要:
        //     The URI for a claim that specifies the method with which an entity was authenticated;
        //     http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod.
        public const string AuthenticationMethod = "authenticationmethod";
        //
        // 摘要:
        //     The URI for a claim that specifies an authorization decision on an entity; http://schemas.xmlsoap.org/ws/2005/05/identity/claims/authorizationdecision.
        public const string AuthorizationDecision = "authorizationdecision";
        //
        // 摘要:
        //     The URI for a claim that specifies the cookie path; http://schemas.microsoft.com/ws/2008/06/identity/claims/cookiepath.
        public const string CookiePath = "cookiepath";
        //
        // 摘要:
        //     The URI for a claim that specifies the country/region in which an entity resides,
        //     http://schemas.xmlsoap.org/ws/2005/05/identity/claims/country.
        public const string Country = "country";
        //
        // 摘要:
        //     The URI for a claim that specifies the date of birth of an entity, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth.
        public const string DateOfBirth = "dateofbirth";
        //
        // 摘要:
        //     The URI for a claim that specifies the deny-only primary group SID on an entity;
        //     http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlyprimarygroupsid.
        //     A deny-only SID denies the specified entity to a securable object.
        public const string DenyOnlyPrimaryGroupSid = "denyonlyprimarygroupsid";
        //
        // 摘要:
        //     The URI for a claim that specifies the deny-only primary SID on an entity; http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlyprimarysid.
        //     A deny-only SID denies the specified entity to a securable object.
        public const string DenyOnlyPrimarySid = "denyonlyprimarysid";
        //
        // 摘要:
        //     The URI for a claim that specifies a deny-only security identifier (SID) for
        //     an entity, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/denyonlysid.
        //     A deny-only SID denies the specified entity to a securable object.
        public const string DenyOnlySid = "denyonlysid";
        //
        // 摘要:
        //     http://schemas.microsoft.com/ws/2008/06/identity/claims/windowsuserclaim.
        public const string WindowsUserClaim = "windowsuserclaim";
        //
        // 摘要:
        //     http://schemas.microsoft.com/ws/2008/06/identity/claims/denyonlywindowsdevicegroup.
        public const string DenyOnlyWindowsDeviceGroup = "denyonlywindowsdevicegroup";
        //
        // 摘要:
        //     http://schemas.microsoft.com/ws/2008/06/identity/claims/dsa.
        public const string Dsa = "dsa";
        //
        // 摘要:
        //     The URI for a claim that specifies the email address of an entity, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/email.
        public const string Email = "emailaddress";
        //
        // 摘要:
        //     http://schemas.microsoft.com/ws/2008/06/identity/claims/expiration.
        public const string Expiration = "expiration";
        //
        // 摘要:
        //     http://schemas.microsoft.com/ws/2008/06/identity/claims/expired.
        public const string Expired = "expired";
        //
        // 摘要:
        //     The URI for a claim that specifies the gender of an entity, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender.
        public const string Gender = "gender";
        //
        // 摘要:
        //     The URI for a claim that specifies the given name of an entity, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname.
        public const string GivenName = "givenname";
        //
        // 摘要:
        //     The URI for a claim that specifies the SID for the group of an entity, http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid.
        public const string GroupSid = "groupsid";
        //
        // 摘要:
        //     The URI for a claim that specifies a hash value, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/hash.
        public const string Hash = "hash";
        //
        // 摘要:
        //     The URI for a claim that specifies the home phone number of an entity, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/homephone.
        public const string HomePhone = "homephone";
        //
        // 摘要:
        //     http://schemas.microsoft.com/ws/2008/06/identity/claims/ispersistent.
        public const string IsPersistent = "ispersistent";
        //
        // 摘要:
        //     The URI for a claim that specifies the locale in which an entity resides, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/locality.
        public const string Locality = "locality";
        //
        // 摘要:
        //     The URI for a claim that specifies the DNS name associated with the computer
        //     name or with the alternative name of either the subject or issuer of an X.509
        //     certificate, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dns.
        public const string Dns = "dns";
        //
        // 摘要:
        //     The URI for a distinguished name claim of an X.509 certificate, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/x500distinguishedname.
        //     The X.500 standard defines the methodology for defining distinguished names that
        //     are used by X.509 certificates.
        public const string X500DistinguishedName = "x500distinguishedname";
    }
}
