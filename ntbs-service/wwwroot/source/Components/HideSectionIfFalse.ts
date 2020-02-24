import Vue from "vue";

const HideSectionIfFalse = Vue.extend({
    methods: {
        hideOrShowSection: function (value: any) {
            if (this.isFalse()) {
                this.$refs["conditional-section"].classList.add("hidden");
            } else {
                this.$refs["conditional-section"].classList.remove("hidden");
            }
        },
        isFalse: function () {
            return this.$refs["inner-validate"].$refs["conditional-false"].checked;
        }
    }
});

export default HideSectionIfFalse;
