set :application, "lg_server"

server "lg.foolishaggro.com", :web, :app, :db, primary: true                          # This may be the same as your `Web` server

set :scm, :none
set :user, "lonelyg"
set :deploy_to, "/home/lonelyg/lg_server"
set :unity, "/Applications/Unity/Unity.app/Contents/MacOS/Unity"
set :project_path, "/Users/Eezo/games/lg"
set :server_repo, "/Users/Eezo/games/lg_server"

namespace :lg do

  task :server, roles: :app do
    system "#{unity} -projectPath #{project_path} -executeMethod AutoBuild.ServerLinux -batchMode -quit"
    system "cd #{server_repo} && git commit -am 'update #{Time.now}' && git push lg"
    run "cd #{deploy_to} && git pull"
    run "cd #{deploy_to} && ./stop_server.sh"
    run "cd #{deploy_to} && ./start_server.sh"
  end

end