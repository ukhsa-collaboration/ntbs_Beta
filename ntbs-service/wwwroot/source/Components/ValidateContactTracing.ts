import Vue from "vue";
import {getHeaders} from "../helpers";
import axios, {Method} from "axios";

type ContactTracingVariables = {
    adultsIdentified: string,
    childrenIdentified: string,
    adultsScreened: string,
    childrenScreened: string,
    adultsLatentTb: string,
    childrenLatentTb: string,
    adultsActiveTb: string,
    childrenActiveTb: string,
    adultsStartedTreatment: string,
    childrenStartedTreatment: string,
    adultsFinishedTreatment: string,
    childrenFinishedTreatment: string
};

const ValidateContactTracing = Vue.extend({
    props: ["model", "property"],
    methods: {
        validateAndCalculateTotals: function () {
            const contactTracingVariables: ContactTracingVariables = this.getContactTracingVariables();
            if (!contactTracingVariables) {
                return;
            }

            this.CalculateTotals(contactTracingVariables);

            this.ResetErrors();

            this.ValidateModel(contactTracingVariables);

        },
        CalculateTotals: function (contactTracingVariables: ContactTracingVariables) {
            this.$refs["totalContactsIdentified"].textContent = parseInt(contactTracingVariables.adultsIdentified) + parseInt(contactTracingVariables.childrenIdentified);
            this.$refs["totalContactsScreened"].textContent = parseInt(contactTracingVariables.adultsScreened) + parseInt(contactTracingVariables.childrenScreened);
            this.$refs["totalContactsActiveTB"].textContent = parseInt(contactTracingVariables.adultsActiveTb) + parseInt(contactTracingVariables.childrenActiveTb);
            this.$refs["totalContactsLatentTB"].textContent = parseInt(contactTracingVariables.adultsLatentTb) + parseInt(contactTracingVariables.childrenLatentTb);
            this.$refs["totalContactsStartedTreatment"].textContent = parseInt(contactTracingVariables.adultsStartedTreatment) + parseInt(contactTracingVariables.childrenStartedTreatment);
            this.$refs["totalContactsFinishedTreatment"].textContent = parseInt(contactTracingVariables.adultsFinishedTreatment) + parseInt(contactTracingVariables.childrenFinishedTreatment);
        },
        ResetErrors: function () {
            const listOfRefs: string[] = ["AdultsIdentified", "ChildrenIdentified", "AdultsScreened", "ChildrenScreened",
                "AdultsActiveTB", "ChildrenActiveTB", "AdultsLatentTB", "ChildrenLatentTB", "AdultsStartedTreatment",
                "ChildrenStartedTreatment", "AdultsFinishedTreatment", "ChildrenFinishedTreatment"];
            for (let i = 0; i < listOfRefs.length; i++) {
                const errorMessageRef = listOfRefs[i] + "ErrorRef";
                const formGroupRef = listOfRefs[i] + "FormGroup";
                const lowerCaseFirstLetterString = listOfRefs[i].charAt(0).toLowerCase() + listOfRefs[i].substring(1);
                this.$refs[errorMessageRef].textContent = "";
                this.$refs[errorMessageRef].classList.add("hidden");
                this.$refs[lowerCaseFirstLetterString].classList.remove("nhsuk-input--error");
                this.$refs[formGroupRef].classList.remove("nhsuk-form-group--error");
            }
        },

        ValidateModel: function (contactTracingVariables: ContactTracingVariables) {
            let requestConfig = {
                method: "post" as Method,
                url: `${this.$props.model}/Validate${this.$props.model}`,
                headers: getHeaders(),
                data: contactTracingVariables
            }

            axios.request(requestConfig)
                .then((response: any) => {
                    var data = response.data;
                    for (let key in data) {
                        if (data.hasOwnProperty(key)) {
                            // Model errors can be double counted as 'Model.Property' and 'Property'.
                            // Here it's convenient to act on 'Property' only
                            if (key.indexOf(".") === -1) {
                                const value = data[key];
                                const errorRef = key + "ErrorRef";
                                const formGroupRef = key + "FormGroup";
                                const lowerCaseFirstLetterString = key.charAt(0).toLowerCase() + key.substring(1);
                                this.$refs[formGroupRef].classList.add("nhsuk-form-group--error");
                                this.$refs[lowerCaseFirstLetterString].classList.add("nhsuk-input--error");
                                this.$refs[errorRef].classList.remove("hidden");
                                this.$refs[errorRef].textContent = value;
                            }
                        }
                    }
                })
                .catch((error: any) => {
                    console.log(error.response);
                });
        },

        getContactTracingVariables: function () {
            const contactTracingVariables: ContactTracingVariables = {
                adultsIdentified: this.$refs["adultsIdentified"].value || 0,
                childrenIdentified: this.$refs["childrenIdentified"].value || 0,
                adultsScreened: this.$refs["adultsScreened"].value || 0,
                childrenScreened: this.$refs["childrenScreened"].value || 0,
                adultsLatentTb: this.$refs["adultsLatentTB"].value || 0,
                childrenLatentTb: this.$refs["childrenLatentTB"].value || 0,
                adultsActiveTb: this.$refs["adultsActiveTB"].value || 0,
                childrenActiveTb: this.$refs["childrenActiveTB"].value || 0,
                adultsStartedTreatment: this.$refs["adultsStartedTreatment"].value || 0,
                childrenStartedTreatment: this.$refs["childrenStartedTreatment"].value || 0,
                adultsFinishedTreatment: this.$refs["adultsFinishedTreatment"].value || 0,
                childrenFinishedTreatment: this.$refs["childrenFinishedTreatment"].value || 0
            };

            return contactTracingVariables;
        }
    }
});

export default ValidateContactTracing;