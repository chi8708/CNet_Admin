<template>
  <div class="user-avatar-dropdown">
    <Dropdown @on-click="handleClick">
      <Badge :dot="!!messageUnreadCount">
        <!-- <Avatar :src="userAvatar"/> -->
        <Avatar :src="headPic"/>
      </Badge>
      <Icon :size="18" type="md-arrow-dropdown"></Icon>
      <DropdownMenu slot="list">
        <!-- <DropdownItem name="message">
          消息中心<Badge style="margin-left: 10px" :count="messageUnreadCount"></Badge>
        </DropdownItem> -->
        <DropdownItem name="editPassword">
          修改密码<Badge style="margin-left: 10px" ></Badge>
        </DropdownItem>
        <DropdownItem name="logout">退出登录</DropdownItem>
      </DropdownMenu>
    </Dropdown>

     <Modal
      title="修改密码"
      :mask-closable="false"
      v-model="modelEditPassword"
      width="800"
      scrollable
      footer-hide
    >
      <edit-password :parent='this' ref="edit"  ></edit-password>
    </Modal>
  </div>
</template>

<script>
import './user.less'
import { mapActions } from 'vuex'
import EditPassword from  './EditPassword.vue'
import headPic from '@/assets/images/header.png'
export default {
  name: 'User',
  components:{
    EditPassword
  },
  data(){
   return {
     modelEditPassword:false,
     headPic:headPic
   }
  },
  props: {
    userAvatar: {
      type: String,
      default: ''
    },
    messageUnreadCount: {
      type: Number,
      default: 0
    }
  },
  methods: {
    ...mapActions([
      'handleLogOut'
    ]),
    logout () {
      this.handleLogOut().then(() => {
        this.$router.push({
          name: 'login'
        })
      })
    },
    message () {
      this.$router.push({
        name: 'message_page'
      })
    },
    editPassword(){
      this.modelEditPassword=true;
    },
    handleClick (name) {
      switch (name) {
        case 'logout': this.logout()
          break
        case 'message': this.message()
          break
        case 'editPassword':this.editPassword()
          break
      }
    }
  }
}
</script>
