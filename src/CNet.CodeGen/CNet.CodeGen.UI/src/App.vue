<template>
  <div class="app-container">
    <el-config-provider>
      <router-view />
    </el-config-provider>
  </div>
</template>

<script setup>
import { ElConfigProvider } from 'element-plus'
</script>

<style>
.app-container {
  width: 100%;
  height: 100vh;
}
</style> 