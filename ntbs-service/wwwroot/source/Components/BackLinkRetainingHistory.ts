import Vue from "vue";

const BackLinkRetainingHistory = Vue.extend({
    methods: {
        navigateBack: function (event: MouseEvent) {
            event.preventDefault();
            history.back();
        }
    }
});

export default BackLinkRetainingHistory;