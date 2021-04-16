# Setting up an AKS cluster

This guide outlines the steps required to setup the NTBS application on a new Kubernetes cluster in Azure.

## Pre-requisites

This guide only covers the Kubernetes resources required. In particular, it assumes the prior existence of: a database server with all of the required databases; and a container registry containing NTBS app images.

Before you begin, you need to generate the required secrets. Assuming that you have access to an existing NTBS cluster, you should export the following secrets, using the command `kubectl get secret <secret-name> -o yaml > <secret-name>.yaml`. For each secret, you should then delete the metadata from the file which is not relevant, e.g. the creation date.
* development-ad-sync-credentials
* int-azuread-options
* int-connection-strings
* registry-password
* softwire-container-registry-secret
* test-azuread-options
* test-connection-strings
* uat-azuread-options
* uat-connection-strings

If you do not have access to an existing NTBS cluster then you will need to populate these files from scratch.

## Steps

The following steps assume that you are working in a resource group called `NTBS_Development` and that you choose to name the cluster `ntbs-envs2`.

1. Create a new Kubernetes cluster using the Azure portal
    * Use a node count of 1 (since this is for dev/test environments only, the load will be low)
    * Include the container registry
    * Otherwise use the default values

1. Enable http-application-routing add on. You can either do this while creating the cluster by choosing "Yes" for "HTTP application routing" on the "Networking" tab, or add it by doing the following:
    * `az aks enable-addons --resource-group NTBS_Development --name ntbs-envs2 --addons http_application_routing`
    * Then get the DNS Zone using `az aks show --resource-group NTBS_Development --name ntbs-envs2 --query addonProfiles.httpApplicationRouting.config.HTTPApplicationRoutingZoneName -o table`
    * For example, `e32846b1ddf0432eb63f.northeurope.aksapp.io`
    * You can also view the DNS Zone in the Azure dashboard (it's a top level resource).

1. Configure `kubectl` to work with the new cluster.
    * Run `az aks get-credentials -g NTBS_Development --name ntbs-envs2` to add credentials and set up a context for the new cluster. This will immediately become the active context.
    * To switch between contexts run `kubectl config use-context <context name>`.

1. Apply all of the secrets mentioned in the pre-requisites into the new cluster, using `kubectl apply -f <secret-name>.yaml`.

1. Now deploy the environments. e.g. For `int` this means you need to:
    * Get the `int.yml` file from the `deployments` directory in the project.
    * Update the domain to the new DNS Zone.
    * `kubectl apply -f int.yml`
    * `kubectl set image deployment/ntbs-int ntbs-int=ntbscontainerregistry.azurecr.io/ntbs-service:<current-build>`

1. Set up the TLS certificates by following the instructions in the cert-manager `readMe`, but remember to update the domains to the new DNS Zone.

1. Update the Azure AD app registrations, adding the new "signin-oidc" route as a redirect URI, for each of the three environments.
 