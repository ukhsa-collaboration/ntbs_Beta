import Vue from "vue";
import ConfirmComponent from "./ConfirmComponent";
require('formdata-polyfill');
require('array-from-polyfill');

const FormLeaveChecker = Vue.extend({
    created(): void {
        window.onclick = this.checkLeave.bind(this);
    },
    mounted: function() {
        this.initialFormData = this.serialiseForm();
    },
    methods: {
        beforeLoad: function () {
            console.log(this.formDataJSON)
        },
        checkLeave: function (event : MouseEvent) {
            const eventTarget = (event.target as HTMLLinkElement);
            if (eventTarget.tagName.toLowerCase() === "a" && eventTarget.parentElement.dataset.ignoreFormLeaveChecker !== "true") {
                if (this.serialiseForm() !== this.initialFormData) {
                    event.preventDefault();
                    
                    this.$modal.confirm(ConfirmComponent, 'Are you sure you want to leave the page without saving changes?')
                        .then(() => {
                            window.location.href = eventTarget.href;
                        })
                        .finally(() => {
                            this.$modal.close()
                        });
                }
            }
        },
        serialiseForm: function () {
            const formData: any = new FormData(this.$refs["notificationForm"]);
            let stringifiedForm = "";
            for (const entry of Array.from(formData.entries())) {
                stringifiedForm += JSON.stringify(entry);
            }
            return stringifiedForm;
        }
    }
});

export default FormLeaveChecker;