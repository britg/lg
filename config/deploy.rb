set :application, "lg_server"

server "lg.foolishaggro.com", :web, :app, :db, primary: true                          # This may be the same as your `Web` server

set :scm, :none
set :user, "lonelyg"
set :deploy_to, "/home/lonelyg/lg_server"
set :unity, "/Applications/Unity/Unity.app/Contents/MacOS/Unity"
set :project_path, "/Users/Eezo/games/lg"
set :server_repo, "/Users/Eezo/games/lg_server"
set :patches_path, "/Users/Eezo/games/lg/Patchie/_output"

namespace :lg do

  task :default do
    admin
    server
    client
  end

  task :server, roles: :app do
    system "#{unity} -projectPath #{project_path} -executeMethod AutoBuild.ServerLinux -batchMode -quit"
    system "cd #{server_repo} && git commit -am 'update #{Time.now}' && git push lg"
    run "cd #{deploy_to} && git pull"
    run "cd #{deploy_to} && ./stop_server.sh"
    run "cd #{deploy_to} && ./start_server.sh"
  end

  task :client do
    write_versions
    upload_new_patches
  end

  task :admin do
    system "cd ../lg_admin && cap deploy"
  end

  task :all do
    admin
    server
    client
  end

end

def last_patch
  versions_txt = open("#{patches_path}/versions.txt"){ |f| f.read }
  versions_txt.split("\n").last.to_s.gsub(" ", "_")
end

def patches
  dirs = Dir.entries(patches_path)
  dirs.select{ |dir| dir.match(/\d+\.\d+\.\d+_\d+\.\d+\.\d+/) }
end

def new_patches
  _patches = patches
  _last = last_patch
  _new = []
  start = !(_last.length > 0)
  _patches.each do |p|
    _new << p if start
    start = true if p == _last
  end
  _new
end

def versions
  v = []
  patches.each do |patch|
    v << patch.split('_').join(" ")
  end
  v
end

def write_versions
  File.open("#{patches_path}/versions.txt", "w") do |f|
    f.write versions.join("\n")
  end
end

def upload_new_patches
  system "rsync -r -v #{patches_path} #{user}@lg.foolishaggro.com:~/apps/lg/shared/system"
end