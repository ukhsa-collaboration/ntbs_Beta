import Vue from "vue";

const SingleSubmitForm = Vue.extend({
    methods: {
        disableButton: function () {
            const button = this.$refs.submitButton
            button.disabled = true
        }
    }
});

export default SingleSubmitForm;