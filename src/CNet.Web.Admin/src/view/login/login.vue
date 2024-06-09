<style lang="less">
  @import './login.less';
</style>

<template>
  <div class="login">
    <div class="login-con">
      <Card icon="log-in" title="欢迎登录" :bordered="false">
        <div class="form-con">
          <login-form @on-success-valid="handleSubmit"></login-form>
          <!-- <p class="login-tip">输入任意用户名和密码即可</p> -->
        </div>
      </Card>
    </div>
  </div>
</template>

<script>
  import LoginForm from '_c/login-form'
  import {
    mapActions
  } from 'vuex'
  export default {
    components: {
      LoginForm
    },
    methods: {
      ...mapActions([
        'handleLogin',
        'getUserInfo'
      ]),
      handleSubmit({
        userName,
        password
      }) {
        if (!this.isEasyPassWord(userName, password)) {
          this.$Message.warning("密码错误!");
          return false
        } else {
          password = hex_md5(password);
          this.handleLogin({
              userName,
              password
            }).then(res => {
              if (res.code != 1) {
                this.$Message.error(res.message);
                return;
              }
              this.getUserInfo().then(res => {
                this.$router.push({
                  name: this.$config.homeName
                })
              })
            })
            .catch(err => {
              this.$Message.error("网络请求出错");
            });
         }
      },
      isEasyPassWord(userName, password) {
        return true;
        if (isCharDanger(userName) != '0') {
          //this.$Message.warning("密码不能包含特殊字符  " + charDanger + "");
          return false
        }
        if (isCharDanger(password) != '0') {
          //this.$Message.warning("密码不能包含特殊字符  " + charDanger + "");
          return false
        }
        if (password.length < 6 || password.length > 10) {
          return false;
        }
        return true;
      }
    }
  }
</script>

<style>

</style>
