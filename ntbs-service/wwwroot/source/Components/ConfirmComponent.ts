import Vue from 'vue'

const confirmTemplate = `
    <div class="dialog-box">
        <section>
            <h3 class="dialog-box-message">{{ message }}</h3>
            <div class="dialog-box-button-container">
                <button class="nhsuk-button nhsuk-button--secondary dialog-box-button" type="button" @click="rejectHandler">Stay</button>
                <button class="nhsuk-button nhsuk-button--secondary dialog-box-button" type="button" @click="resolveHandler">Leave</button>
            </div>
        </section>
    </div>
    `;

export default Vue.extend({
    name: 'ConfirmComponent',
    template: confirmTemplate,
    props: {
        message: {
            type: String,
            required: true,
        },
        resolve: {
            type: Function,
            required: true,
        },
        reject: {
            type: Function,
            required: true,
        },
    },
    methods: {
        resolveHandler() {
            // Resolve promise within vue-accessible-modal
            // In this implementation triggers .Confirm().then() block in FormLeaveChecker
            this.resolve();
        },
        rejectHandler() {
            // Reject promise within vue-accessible-modal
            // In this implementation triggers .Confirm().catch() block in FormLeaveChecker 
            this.reject();
        },
    },
});