<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="Dissertation" generation="1" functional="0" release="0" Id="0d751d0f-ce25-46cf-8536-56cb6b3445b8" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="DissertationGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="WebService:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/Dissertation/DissertationGroup/LB:WebService:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="WebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp">
          <inToChannel>
            <lBChannelMoniker name="/Dissertation/DissertationGroup/LB:WebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Certificate|WebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" defaultValue="">
          <maps>
            <mapMoniker name="/Dissertation/DissertationGroup/MapCertificate|WebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </maps>
        </aCS>
        <aCS name="Scheduler:Microsoft.ServiceBus.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Dissertation/DissertationGroup/MapScheduler:Microsoft.ServiceBus.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="Scheduler:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Dissertation/DissertationGroup/MapScheduler:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="SchedulerInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/Dissertation/DissertationGroup/MapSchedulerInstances" />
          </maps>
        </aCS>
        <aCS name="WebService:Microsoft.ServiceBus.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Dissertation/DissertationGroup/MapWebService:Microsoft.ServiceBus.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="WebService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/Dissertation/DissertationGroup/MapWebService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="WebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="">
          <maps>
            <mapMoniker name="/Dissertation/DissertationGroup/MapWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </maps>
        </aCS>
        <aCS name="WebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="">
          <maps>
            <mapMoniker name="/Dissertation/DissertationGroup/MapWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </maps>
        </aCS>
        <aCS name="WebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="">
          <maps>
            <mapMoniker name="/Dissertation/DissertationGroup/MapWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </maps>
        </aCS>
        <aCS name="WebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/Dissertation/DissertationGroup/MapWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </maps>
        </aCS>
        <aCS name="WebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="">
          <maps>
            <mapMoniker name="/Dissertation/DissertationGroup/MapWebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </maps>
        </aCS>
        <aCS name="WebServiceInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/Dissertation/DissertationGroup/MapWebServiceInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:WebService:Endpoint1">
          <toPorts>
            <inPortMoniker name="/Dissertation/DissertationGroup/WebService/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:WebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput">
          <toPorts>
            <inPortMoniker name="/Dissertation/DissertationGroup/WebService/Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </toPorts>
        </lBChannel>
        <sFSwitchChannel name="SW:WebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp">
          <toPorts>
            <inPortMoniker name="/Dissertation/DissertationGroup/WebService/Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
          </toPorts>
        </sFSwitchChannel>
      </channels>
      <maps>
        <map name="MapCertificate|WebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" kind="Identity">
          <certificate>
            <certificateMoniker name="/Dissertation/DissertationGroup/WebService/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
          </certificate>
        </map>
        <map name="MapScheduler:Microsoft.ServiceBus.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Dissertation/DissertationGroup/Scheduler/Microsoft.ServiceBus.ConnectionString" />
          </setting>
        </map>
        <map name="MapScheduler:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Dissertation/DissertationGroup/Scheduler/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapSchedulerInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/Dissertation/DissertationGroup/SchedulerInstances" />
          </setting>
        </map>
        <map name="MapWebService:Microsoft.ServiceBus.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Dissertation/DissertationGroup/WebService/Microsoft.ServiceBus.ConnectionString" />
          </setting>
        </map>
        <map name="MapWebService:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/Dissertation/DissertationGroup/WebService/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" kind="Identity">
          <setting>
            <aCSMoniker name="/Dissertation/DissertationGroup/WebService/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" />
          </setting>
        </map>
        <map name="MapWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" kind="Identity">
          <setting>
            <aCSMoniker name="/Dissertation/DissertationGroup/WebService/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" />
          </setting>
        </map>
        <map name="MapWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" kind="Identity">
          <setting>
            <aCSMoniker name="/Dissertation/DissertationGroup/WebService/Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" />
          </setting>
        </map>
        <map name="MapWebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/Dissertation/DissertationGroup/WebService/Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" />
          </setting>
        </map>
        <map name="MapWebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" kind="Identity">
          <setting>
            <aCSMoniker name="/Dissertation/DissertationGroup/WebService/Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" />
          </setting>
        </map>
        <map name="MapWebServiceInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/Dissertation/DissertationGroup/WebServiceInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="Scheduler" generation="1" functional="0" release="0" software="C:\Users\Marc.COOPERSOFTWARE\Dropbox\Dissertation\Code\Dissertation\Dissertation\csx\Debug\roles\Scheduler" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <componentports>
              <outPort name="WebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/Dissertation/DissertationGroup/SW:WebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.ServiceBus.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;Scheduler&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;Scheduler&quot; /&gt;&lt;r name=&quot;WebService&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/Dissertation/DissertationGroup/SchedulerInstances" />
            <sCSPolicyFaultDomainMoniker name="/Dissertation/DissertationGroup/SchedulerFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="WebService" generation="1" functional="0" release="0" software="C:\Users\Marc.COOPERSOFTWARE\Dropbox\Dissertation\Code\Dissertation\Dissertation\csx\Debug\roles\WebService" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" protocol="tcp" />
              <inPort name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp" portRanges="3389" />
              <outPort name="WebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" protocol="tcp">
                <outToChannel>
                  <sFSwitchChannelMoniker name="/Dissertation/DissertationGroup/SW:WebService:Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp" />
                </outToChannel>
              </outPort>
            </componentports>
            <settings>
              <aCS name="Microsoft.ServiceBus.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountEncryptedPassword" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountExpiration" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.AccountUsername" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteAccess.Enabled" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.RemoteForwarder.Enabled" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;WebService&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;Scheduler&quot; /&gt;&lt;r name=&quot;WebService&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteAccess.Rdp&quot; /&gt;&lt;e name=&quot;Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="WebService.svclog" defaultAmount="[1000,1000,1000]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/Dissertation/DissertationGroup/WebService/Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="Microsoft.WindowsAzure.Plugins.RemoteAccess.PasswordEncryption" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/Dissertation/DissertationGroup/WebServiceInstances" />
            <sCSPolicyFaultDomainMoniker name="/Dissertation/DissertationGroup/WebServiceFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyFaultDomain name="SchedulerFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="WebServiceFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="SchedulerInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="WebServiceInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="908fcb5d-8f0c-4e00-87e5-8d0f3604859c" ref="Microsoft.RedDog.Contract\ServiceContract\DissertationContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="34b64f1f-3b0c-4449-bccc-5c27ab6c15d1" ref="Microsoft.RedDog.Contract\Interface\WebService:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/Dissertation/DissertationGroup/WebService:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="ceddb0fe-e0e5-4a09-bdfe-2844bd327e14" ref="Microsoft.RedDog.Contract\Interface\WebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/Dissertation/DissertationGroup/WebService:Microsoft.WindowsAzure.Plugins.RemoteForwarder.RdpInput" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>