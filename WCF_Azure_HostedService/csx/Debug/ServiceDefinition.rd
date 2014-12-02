<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="WCF_Azure_HostedService" generation="1" functional="0" release="0" Id="668b8848-c1c5-4834-b31e-c0e7d210de87" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="WCF_Azure_HostedServiceGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="WCF_Azure_Service:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/WCF_Azure_HostedService/WCF_Azure_HostedServiceGroup/LB:WCF_Azure_Service:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="WCF_Azure_ServiceInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/WCF_Azure_HostedService/WCF_Azure_HostedServiceGroup/MapWCF_Azure_ServiceInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:WCF_Azure_Service:Endpoint1">
          <toPorts>
            <inPortMoniker name="/WCF_Azure_HostedService/WCF_Azure_HostedServiceGroup/WCF_Azure_Service/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapWCF_Azure_ServiceInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/WCF_Azure_HostedService/WCF_Azure_HostedServiceGroup/WCF_Azure_ServiceInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="WCF_Azure_Service" generation="1" functional="0" release="0" software="C:\Users\Tom\Desktop\PRApplicationFainal\WCF_Azure_HostedService\csx\Debug\roles\WCF_Azure_Service" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;WCF_Azure_Service&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;WCF_Azure_Service&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/WCF_Azure_HostedService/WCF_Azure_HostedServiceGroup/WCF_Azure_ServiceInstances" />
            <sCSPolicyUpdateDomainMoniker name="/WCF_Azure_HostedService/WCF_Azure_HostedServiceGroup/WCF_Azure_ServiceUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/WCF_Azure_HostedService/WCF_Azure_HostedServiceGroup/WCF_Azure_ServiceFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="WCF_Azure_ServiceUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="WCF_Azure_ServiceFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="WCF_Azure_ServiceInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="814aeb4c-9598-464e-98fc-15e2285c2998" ref="Microsoft.RedDog.Contract\ServiceContract\WCF_Azure_HostedServiceContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="44d2b3ef-98b4-4a28-a31b-5923cdbd654f" ref="Microsoft.RedDog.Contract\Interface\WCF_Azure_Service:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/WCF_Azure_HostedService/WCF_Azure_HostedServiceGroup/WCF_Azure_Service:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>