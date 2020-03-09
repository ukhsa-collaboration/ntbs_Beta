import Vue from "vue";
import ConfirmComponent from "./ConfirmComponent";
import 'formdata-polyfill';

const FormLeaveChecker = Vue.extend({
    render(): any { return null },
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
                        .catch(() => {
                            this.$modal.close()
                        });
                }
            }
        },
        serialiseForm: function () {
            const forms : any = document.querySelectorAll('form');
            let stringifiedForm = "";

            for (let index = 0; index < forms.length; index++) {
                const formData: any = new FormData(forms[index]);
                const allEntries = formData.entries();
                
                let currentValue = allEntries.next();
                while (!currentValue.done)
                {
                    stringifiedForm += JSON.stringify(currentValue);
                    currentValue = allEntries.next();
                }
            }
            
            return stringifiedForm;
        }
    }
});

export default FormLeaveChecker;