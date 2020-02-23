import Vue from 'vue'

const confirmTemplate = `
    <div class="dialog-box">
        <template>
            <section>
                <h3 class="dialog-box-message">{{ message }}</h3>
                <div class="dialog-box-button-container">
                    <button class="nhsuk-button nhsuk-button--secondary dialog-box-button" type="button" @click="rejectHandler">Stay</button>
                    <button class="nhsuk-button nhsuk-button--secondary dialog-box-button" type="button" @click="resolveHandler">Leave</button>
                </div>
            </section>
        </template>
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
            // this value will be passed to `then`
            this.resolve()
        },
        rejectHandler() {
            // this value will be passed to `catch`
            this.reject()
        },
    },
});