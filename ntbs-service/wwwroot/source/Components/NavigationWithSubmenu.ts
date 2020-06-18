import Vue from "vue";

const NavigationWithSubmenu = Vue.extend({
    created(): void {
        window.onclick = this.hideNavDropdown.bind(this);
    },
    methods: {
        hideNavDropdown: function (event: MouseEvent )
        {
            let inputElement = (<HTMLInputElement>event.target);
            let headerList = document.getElementsByClassName('nav-with-submenu-with-js');
            
            for (let i = 0; i < headerList.length; i++) {
                let headerLink = headerList[i].getElementsByClassName('nav-with-submenu-header-link')[0];
                let dropdown = headerList[i].getElementsByClassName('nav-submenu-list')[0];
                if (inputElement != headerLink && dropdown.classList.contains('show')) {
                    dropdown.classList.remove('show');
                }
            }
        },
        toggleMenu: function () {
            this.$refs["navigation-submenu"].classList.toggle("show");
        }
    }
});

export default NavigationWithSubmenu;