import Vue from 'vue'
import Vuetify from 'vuetify'
import 'vuetify/dist/vuetify.min.css'
import colors from 'vuetify/es5/util/colors'
import { Iconfont } from 'vuetify/types/services/icons'

Vue.use(Vuetify)

const opts = {
    theme: {
        themes:{
            light:{
                // primary: colors.teal.base,
                // secondary: colors.cyan.base,
                // accent: colors.red.base,
                // error: colors.purple.base,
                // warning: colors.blue.base,
                // info: colors.deepPurple.base,
                // success: colors.lightGreen.base

                primary: colors.teal.base,
                secondary: colors.cyan.base,
                accent: colors.blueGrey.base,
                error: colors.deepOrange.base,
                warning: colors.orange.base,
                info: colors.blue.base,
                success: colors.green.base
            },
        }
        
    },
}

export default new Vuetify(opts)