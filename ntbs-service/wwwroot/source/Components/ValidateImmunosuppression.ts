import Vue from "vue";
import axios from "axios";
import { getHeaders } from "../helpers";

const ValidateImmunosuppression = Vue.extend({
    methods: {
        validate(event: FocusEvent) {
            // Do nothing if focused element is part of the div that lost focus
            if (this.$el.contains(event.relatedTarget)) {
                return;
            }

            const inputValues = this.getInputValues();

            const requestConfig = {
                url: "Immunosuppression/Validate",
                headers: getHeaders(),
                params: inputValues
            };

            axios.request(requestConfig)
                .then((response: any) => {
                    const data = response.data;

                    if (data["ImmunosuppressionDetails.Status"]) {
                        const errorMessage = data["ImmunosuppressionDetails.Status"];
                        this.$refs.statusFormGroup.classList.add("nhsuk-form-group--error");
                        this.$refs.statusError.textContent = errorMessage;
                    } else {
                        this.$refs.statusFormGroup.classList.remove("nhsuk-form-group--error");
                        this.$refs.statusError.textContent = "";
                    }

                    if (data["ImmunosuppressionDetails.OtherDescription"]) {
                        const errorMessage = data["ImmunosuppressionDetails.OtherDescription"];
                        this.$refs.descriptionFormGroup.classList.add("nhsuk-form-group--error");
                        this.$refs.otherDescription.classList.add("nhsuk-input--error");
                        this.$refs.descriptionError.textContent = errorMessage;
                    } else {
                        this.$refs.descriptionFormGroup.classList.remove("nhsuk-form-group--error");
                        this.$refs.otherDescription.classList.remove("nhsuk-input--error");
                        this.$refs.descriptionError.textContent = "";
                    }
                })
                .catch((error: any) => {
                    console.log(error.response);
                });
        },

        getStatus() {
            const statusYes = this.$refs.statusYes.checked;
            const statusNo = this.$refs.statusNo.checked;
            const statusUnknown = this.$refs.statusUnknown.checked;

            return statusYes ? "Yes" : statusNo ? "No" : statusUnknown ? "Unknown" : "";
        },

        getInputValues() {
            return {
                status : this.getStatus(),

                hasBioTherapy: this.$refs.hasBioTherapy.checked,
                hasTransplantation: this.$refs.hasTransplantation.checked,
                hasOther: this.$refs.hasOther.checked,

                otherDescription: this.$refs.otherDescription.value
            }
        }
    }
});

export default ValidateImmunosuppression;
